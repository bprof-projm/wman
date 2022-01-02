import 'package:bot_toast/bot_toast.dart';
import 'package:flutter/material.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:http/http.dart' as http;
import 'package:intl/intl.dart';
import 'package:signalr_core/signalr_core.dart';
import 'package:wman_mobile/classes/Const/apiAcess.dart';
import 'package:wman_mobile/classes/signalR.dart';
import 'package:wman_mobile/widgets/labelListScroller.dart';
import 'package:wman_mobile/widgets/login.dart';
import 'package:wman_mobile/widgets/secondpage.dart';
import '../classes/workEventCard.dart';
import 'package:wman_mobile/widgets/workCard_detail.dart';

class WorkList extends StatefulWidget {
  @override
  WorkListState createState() => WorkListState();
}

class WorkListState extends State<WorkList> {
  FlutterLocalNotificationsPlugin flutterLocalNotificationsPlugin =
      FlutterLocalNotificationsPlugin();
// initialise the plugin. app_icon needs to be a added as a drawable resource to the Android head project
  static const AndroidInitializationSettings initializationSettingsAndroid =
      AndroidInitializationSettings('background');
  InitializationSettings initializationSettings =
      const InitializationSettings(android: initializationSettingsAndroid);

  List<WorkEventCard> workEventCards = List.empty();
  bool _isLoading = true;

  void _delKey() async {
    if (SignalRAccess.signalr != null) {
      await SignalRAccess.signalr!.hubConnection.stop();
    }
    setState(() {
      _isLoading = true;
    });
    await ApiAccess.secureStorage.delete(key: 'jwt');
    setState(() {
      _isLoading = false;
    });
    Navigator.pushNamed(context, '/');
  }

  void selectNotification(String? payload) async {
    if (payload != null) {
      debugPrint('notification payload: $payload');
    }
    await Navigator.push(
      context,
      MaterialPageRoute<void>(builder: (context) => SecondPage(payload)),
    );
  }

  void _notificationNavigate(String? payload) async {
    if (payload != null) {
      debugPrint('notification payload: $payload');
    }
    WorkEventCard fromPayload = workEventFromJson(payload!);
    WorkEventCard toNavigate = workEventCards
        .singleWhere((element) => element.id == fromPayload.id, orElse: () {
      return fromPayload;
    });
    print("EZ: " + toNavigate.toJson().toString());
    await Navigator.push(
      context,
      MaterialPageRoute<void>(
          builder: (context) => WorkCardDetail(
                workData: toNavigate,
                valueChanged: _valueChanged,
              )),
    );
  }

  _notification() async {
    BotToast.showSimpleNotification(
        title: "NOTIFY", duration: Duration(seconds: 3));
    const AndroidNotificationDetails androidPlatformChannelSpecifics =
        AndroidNotificationDetails('your channel id', 'your channel name',
            channelDescription: 'your channel description',
            importance: Importance.max,
            priority: Priority.high,
            ticker: 'ticker');
    const NotificationDetails platformChannelSpecifics =
        NotificationDetails(android: androidPlatformChannelSpecifics);
    await flutterLocalNotificationsPlugin.show(
        0, 'plain title', 'plain body', platformChannelSpecifics,
        payload: 'item x');
    // BotToast.showAttachedWidget(
    //     attachedBuilder: (_) => Card(
    //           child: const Padding(
    //             padding: EdgeInsets.all(8.0),
    //             child: Icon(
    //               Icons.favorite,
    //               color: Colors.redAccent,
    //             ),
    //           ),
    //         ),
    //     duration: Duration(seconds: 2),
    //     target: Offset(520, 520));
  }

  Future<void> _syncApiCall() async {
    print('Loading');
    String? token = await ApiAccess.secureStorage.read(key: 'jwt');
    http.Response response = await http.get(
      //Uri.parse("https://127.0.0.1:5001/Worker/events/today"),
      Uri.parse(ApiAccess.baseUrl + "/CalendarEvent/GetCurrentDayEvents"),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
        'Authorization': 'Bearer $token',
      },
    );
    if (response.statusCode == 200) {
      // List<WorkerEventCard> responseBody = jsonDecode(response.body);
      workEventCards = workEventCardFromJson(response.body);
      print(workEventCards.isEmpty);
    } else if (response.statusCode == 401) {
      SignalRAccess.signalr = null;
      BotToast.showText(text: "You have been logged out");
      _delKey();
      print(response.statusCode);
    }
    print(response.statusCode);
    // print("GET BODY: " + response.body);
    _isLoading = false;
  }

  Future<void> fetch() async {
    //asd
    await flutterLocalNotificationsPlugin.initialize(initializationSettings,
        onSelectNotification: _notificationNavigate);
    // method
    await _syncApiCall();
    //SignalRAccess.signalr.initSignalR();
    if (SignalRAccess.signalr == null) {
      SignalRAccess.signalr = SignalRService();
      SignalRAccess.signalr!.initSignalR();
    }
    SignalRAccess.signalr!.hubConnection.state != HubConnectionState.connected
        ? {
            await SignalRAccess.signalr!.hubConnection.start(),
            print('Connection is up: ' +
                '${SignalRAccess.signalr!.hubConnection.state == HubConnectionState.connected}'),
          }
        : {};
    SignalRAccess.signalr!.hubConnection.on("UserAssignedCurrentDay",
        (arguments) {
      _notifyEventAssign(arguments);
    });
    SignalRAccess.signalr!.hubConnection.on("EventChangedForToday",
        (arguments) {
      _notifyEventChange(arguments);
    });
    SignalRAccess.signalr!.hubConnection.on("EventChangedFromTodayToNotToday",
        (arguments) {
      _notifyEventChangeFromCurrent(arguments);
    });
    SignalRAccess.signalr!.hubConnection.on("EventStateChanged", (arguments) {
      _notifyEventChangeState(arguments);
    });
    setState(() {
      _isLoading = false;
    });

    // String asd =
    //     '{"id":3,"jobDescription":"Example event #3","estimatedStartDate":"2021-12-27T22:00:00","estimatedFinishDate":"2021-12-27T23:00:00","assignedUsers":[{"username":"user1","email":"pelda@abc.com","firstname":"First","lastname":"User","profilePicture":{"cloudPhotoID":null,"url":"https://res.cloudinary.com/wmanproj/image/upload/v1640115283/ang5fv0sghevcrcs6ayp.png","wManUserID":3},"phoneNumber":"0690123456"}],"labels":[],"address":{"city":"Budapest","street":"Doberd\u00f3 \u00fat","zipCode":1034,"buildingNumber":"6/A.","floorDoor":null},"workStartDate":"2021-12-22T05:10:00","workFinishDate":"2021-12-22T08:30:00","status":"2"}';
    // WorkEventCard v = workEventFromJson(asd);
    // print(v.jobDescription);
  }

  WorkListState() {
    fetch();
  }
  _notifyEventAssign(List<dynamic>? arguments) async {
    setState(() {
      _isLoading = true;
    });
    var response = arguments![0].toString();
    print("RESPONSE: " + response);
    // Map<String, dynamic> asd = jsonDecode(arguments![0]);
    WorkEventCard v = workEventFromJson(response.toString());
    print(v.jobDescription);
    // print(v.jobDescription);
    setState(() {
      this.workEventCards.add(v);
      this
          .workEventCards
          .sort((a, b) => a.estimatedStartDate.compareTo(b.estimatedStartDate));
      // BotToast.showSimpleNotification(
      //     title: "Rá lettél rakva egy eventre",
      //     duration: Duration(seconds: 10));
      _isLoading = false;
    });
    const AndroidNotificationDetails androidPlatformChannelSpecifics =
        AndroidNotificationDetails('your channel id', 'your channel name',
            channelDescription: 'your channel description',
            importance: Importance.max,
            priority: Priority.high,
            ticker: 'ticker');
    const NotificationDetails platformChannelSpecifics =
        NotificationDetails(android: androidPlatformChannelSpecifics);
    await flutterLocalNotificationsPlugin.show(0, 'WMAN Mobile',
        'You have been assigned to a job.', platformChannelSpecifics,
        payload: response.toString());
  }

  _notifyEventChange(List<dynamic>? arguments) async {
    setState(() {
      _isLoading = true;
    });
    var response = arguments![0].toString();
    print("RESPONSE: " + response);
    // Map<String, dynamic> asd = jsonDecode(arguments![0]);
    WorkEventCard v = workEventFromJson(response.toString());
    print(v.jobDescription);
    // print(v.jobDescription);
    setState(() {
      this.workEventCards.removeWhere((element) => element.id == v.id);
      this.workEventCards.add(v);
      this
          .workEventCards
          .sort((a, b) => a.estimatedStartDate.compareTo(b.estimatedStartDate));
      // BotToast.showSimpleNotification(
      //     title: "Rá lettél rakva egy eventre",
      //     duration: Duration(seconds: 10));
      _isLoading = false;
    });
    const AndroidNotificationDetails androidPlatformChannelSpecifics =
        AndroidNotificationDetails('your channel id', 'your channel name',
            channelDescription: 'your channel description',
            importance: Importance.max,
            priority: Priority.high,
            ticker: 'ticker');
    const NotificationDetails platformChannelSpecifics =
        NotificationDetails(android: androidPlatformChannelSpecifics);
    await flutterLocalNotificationsPlugin.show(0, 'WMAN Mobile',
        'Your job has been modified.', platformChannelSpecifics,
        payload: response.toString());
  }

  _notifyEventChangeFromCurrent(List<dynamic>? arguments) async {
    setState(() {
      _isLoading = true;
    });
    var response = arguments![0].toString();
    print("RESPONSE: " + response);
    WorkEventCard v = workEventFromJson(response.toString());
    print(v.jobDescription);
    setState(() {
      this.workEventCards.removeWhere((element) => element.id == v.id);
      _isLoading = false;
    });
    const AndroidNotificationDetails androidPlatformChannelSpecifics =
        AndroidNotificationDetails('your channel id', 'your channel name',
            channelDescription: 'your channel description',
            importance: Importance.max,
            priority: Priority.high,
            ticker: 'ticker');
    const NotificationDetails platformChannelSpecifics =
        NotificationDetails(android: androidPlatformChannelSpecifics);
    await flutterLocalNotificationsPlugin.show(0, 'WMAN Mobile',
        'Your job has been moved to another day.', platformChannelSpecifics,
        payload: response.toString());
  }

  _notifyEventChangeState(List<dynamic>? arguments) async {
    setState(() {
      _isLoading = true;
    });
    var response = arguments![0].toString();
    print("RESPONSE: " + response);
    // Map<String, dynamic> asd = jsonDecode(arguments![0]);
    WorkEventCard v = workEventFromJson(response.toString());
    print(v.jobDescription);
    // print(v.jobDescription);
    setState(() {
      this.workEventCards.removeWhere((element) => element.id == v.id);
      this.workEventCards.add(v);
      this
          .workEventCards
          .sort((a, b) => a.estimatedStartDate.compareTo(b.estimatedStartDate));
      _isLoading = false;
    });
    const AndroidNotificationDetails androidPlatformChannelSpecifics =
        AndroidNotificationDetails('your channel id', 'your channel name',
            channelDescription: 'your channel description',
            importance: Importance.max,
            priority: Priority.high,
            ticker: 'ticker');
    const NotificationDetails platformChannelSpecifics =
        NotificationDetails(android: androidPlatformChannelSpecifics);
    await flutterLocalNotificationsPlugin.show(
        0,
        'WMAN Mobile',
        'One of your coworker has modified the state of your job.',
        platformChannelSpecifics,
        payload: response.toString());
  }

  void _printKey() async {
    String? value = await ApiAccess.secureStorage.read(key: 'jwt');
    // print(value);
  }

  void _valueChanged(WorkEventCard workData) {
    print("BEJÖTT");
    setState(() {
      _isLoading = true;
    });
    setState(() {
      this.workEventCards.removeWhere((element) => element.id == workData.id);
      this.workEventCards.add(workData);
      this
          .workEventCards
          .sort((a, b) => a.estimatedStartDate.compareTo(b.estimatedStartDate));
      _isLoading = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    ScreenUtil.init(
      BoxConstraints(
          maxWidth: MediaQuery.of(context).size.width,
          maxHeight: MediaQuery.of(context).size.height),
      designSize: Size(360, 690),
      orientation: Orientation.portrait,
    );

    return WillPopScope(
      onWillPop: () async => false,
      child: Scaffold(
        appBar: AppBar(
          title: const Text('Manage your jobs'),
          automaticallyImplyLeading: false,
        ),
        body: _isLoading
            ? const Center(
                child: CircularProgressIndicator(),
              )
            // : Column(
            //     children: [
            //       ElevatedButton(
            //         onPressed: _printKey,
            //         child: Text('GET JWT'),
            //       ),
            //       const Text('You are now logged in!'),
            //       ElevatedButton(
            //         onPressed: _delKey,
            //         child: Text('Logout'),
            //       ),
            //    const Text('You are now logged in!'),
            : Stack(
                children: [
                  workEventCards.isEmpty
                      ? Center(
                          child: Text(
                            "You have no assigned works for today",
                            style: TextStyle(
                              fontSize: 20.w < 10
                                  ? 10
                                  : 20.w > 31
                                      ? 31
                                      : 20.w,
                              color: Colors.grey,
                            ),
                            textAlign: TextAlign.center,
                          ),
                        )
                      : Column(
                          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                          children: [
                            Expanded(
                              child: ListView.builder(
                                itemBuilder: (ctx, index) {
                                  return SizedBox(
                                    width: 250.w < 250
                                        ? 250
                                        : 250.w > 312
                                            ? 312
                                            : 250.w,
                                    child: Card(
                                      elevation: 3,
                                      child: Padding(
                                        padding: EdgeInsets.only(bottom: 10),
                                        child: Column(
                                          crossAxisAlignment:
                                              CrossAxisAlignment.start,
                                          children: [
                                            Container(
                                              width: double.infinity,
                                              height: 22.w < 22
                                                  ? 22
                                                  : 22.w > 27
                                                      ? 27
                                                      : 22.w,
                                              decoration: BoxDecoration(
                                                  borderRadius:
                                                      BorderRadius.circular(5),
                                                  color: Styling.statuscolor(
                                                      workEventCards[index]
                                                          .status)),
                                              child: Center(
                                                child: FittedBox(
                                                  fit: BoxFit.fitWidth,
                                                  child: Text(
                                                    workEventCards[index]
                                                        .jobDescription,
                                                    style: TextStyle(
                                                      fontSize: 22.w < 22
                                                          ? 22
                                                          : 22.w > 27
                                                              ? 27
                                                              : 22.w,
                                                    ),
                                                  ),
                                                ),
                                              ),
                                            ),
                                            Container(
                                              margin: EdgeInsets.all(10),
                                              child: Column(
                                                crossAxisAlignment:
                                                    CrossAxisAlignment.stretch,
                                                children: [
                                                  Text(
                                                      workEventCards[index]
                                                              .address
                                                              .zipCode
                                                              .toString() +
                                                          ', ' +
                                                          workEventCards[index]
                                                              .address
                                                              .city,
                                                      style: TextStyle(
                                                          fontSize: 18.w < 18
                                                              ? 18
                                                              : 18.w > 20
                                                                  ? 20
                                                                  : 18.w)),
                                                  Text(
                                                    '${workEventCards[index].address.street}'
                                                    ' '
                                                    '${workEventCards[index].address.buildingNumber}'
                                                    ' '
                                                    '${workEventCards[index].address.floorDoor ?? ''}', // Display text if not null
                                                    style: TextStyle(
                                                        fontSize: 18.w < 18
                                                            ? 18
                                                            : 18.w > 20
                                                                ? 20
                                                                : 18.w),
                                                  )
                                                ],
                                              ),
                                            ),
                                            Center(
                                              child: Text(
                                                DateFormat.Hm().format(
                                                        workEventCards[index]
                                                            .estimatedStartDate) +
                                                    ' - ' +
                                                    DateFormat.Hm().format(
                                                        workEventCards[index]
                                                            .estimatedFinishDate),
                                                style: TextStyle(
                                                  fontSize: 23.w < 23
                                                      ? 23
                                                      : 23.w > 28
                                                          ? 28
                                                          : 23.w,
                                                ),
                                              ),
                                            ),
                                            LabelListScroller(
                                                workEventCards[index].labels),
                                            Row(
                                              mainAxisAlignment:
                                                  MainAxisAlignment.end,
                                              children: [
                                                Padding(
                                                  padding:
                                                      EdgeInsets.only(left: 25),
                                                  child: SizedBox(
                                                    width: 100.w < 75
                                                        ? 75
                                                        : 100.w > 125
                                                            ? 125
                                                            : 100.w,
                                                    height: 50.w < 12
                                                        ? 12
                                                        : 50.w > 55
                                                            ? 55
                                                            : 50.w,
                                                    child: ElevatedButton(
                                                      onPressed: () {
                                                        Navigator.push(
                                                            context,
                                                            MaterialPageRoute(
                                                              builder: (context) =>
                                                                  WorkCardDetail(
                                                                workData:
                                                                    workEventCards[
                                                                        index],
                                                                valueChanged:
                                                                    _valueChanged,
                                                              ),
                                                            ));
                                                      },
                                                      child:
                                                          Text('View more...'),
                                                    ),
                                                  ),
                                                ),
                                                Flexible(
                                                  fit: FlexFit.tight,
                                                  child: SizedBox(),
                                                ),
                                                Container(
                                                  margin: EdgeInsets.only(
                                                      left: 5.w, right: 5.w),
                                                  child: SizedBox(
                                                    width: 50.w < 30
                                                        ? 30
                                                        : 50.w > 60
                                                            ? 60
                                                            : 50.w,
                                                    height: 50.w < 30
                                                        ? 30
                                                        : 50.w > 60
                                                            ? 60
                                                            : 50.w,
                                                    child: DecoratedBox(
                                                      decoration: BoxDecoration(
                                                        shape: BoxShape.circle,
                                                        border: Border.all(
                                                            color: Colors.black,
                                                            width: 1.0,
                                                            style: BorderStyle
                                                                .solid),
                                                        image: DecorationImage(
                                                          image: workEventCards[
                                                                          index]
                                                                      .assignedUsers[
                                                                          0]
                                                                      .profilePicture ==
                                                                  null
                                                              ? const AssetImage(
                                                                      "assets/wman_profpic_placeholder.png")
                                                                  as ImageProvider
                                                              : NetworkImage(
                                                                  workEventCards[
                                                                          index]
                                                                      .assignedUsers[
                                                                          0]
                                                                      .profilePicture!
                                                                      .url),
                                                        ),
                                                      ),
                                                    ),
                                                  ),
                                                ),
                                                workEventCards[index]
                                                            .assignedUsers
                                                            .length >
                                                        1
                                                    ? Container(
                                                        alignment: Alignment
                                                            .bottomRight,
                                                        child: SizedBox(
                                                          width: 50.w < 30
                                                              ? 30
                                                              : 50.w > 60
                                                                  ? 60
                                                                  : 50.w,
                                                          height: 50.w < 30
                                                              ? 30
                                                              : 50.w > 60
                                                                  ? 60
                                                                  : 50.w,
                                                          child: DecoratedBox(
                                                            decoration:
                                                                BoxDecoration(
                                                              shape: BoxShape
                                                                  .circle,
                                                              border: Border.all(
                                                                  color: Colors
                                                                      .black,
                                                                  width: 1.0,
                                                                  style:
                                                                      BorderStyle
                                                                          .solid),
                                                              image:
                                                                  DecorationImage(
                                                                image: workEventCards[index]
                                                                            .assignedUsers[
                                                                                1]
                                                                            .profilePicture ==
                                                                        null
                                                                    ? const AssetImage(
                                                                            "assets/wman_profpic_placeholder.png")
                                                                        as ImageProvider
                                                                    : NetworkImage(workEventCards[
                                                                            index]
                                                                        .assignedUsers[
                                                                            1]
                                                                        .profilePicture!
                                                                        .url),
                                                              ),
                                                            ),
                                                          ),
                                                        ),
                                                      )
                                                    : const SizedBox.shrink(),
                                                workEventCards[index]
                                                            .assignedUsers
                                                            .length >
                                                        2
                                                    ? Padding(
                                                        padding:
                                                            EdgeInsets.only(
                                                                left: 10,
                                                                right: 10),
                                                        child: Text(
                                                          "+${workEventCards[index].assignedUsers.length - 2}",
                                                          style: TextStyle(
                                                              fontSize: 25.w <
                                                                      15
                                                                  ? 15
                                                                  : 25.w > 35
                                                                      ? 35
                                                                      : 25.w),
                                                        ),
                                                      )
                                                    : const SizedBox.shrink(),
                                              ],
                                            ),
                                          ],
                                        ),
                                      ),
                                    ),
                                  );
                                },
                                itemCount: workEventCards.length,
                              ),
                            ),
                          ],
                        ),
                  Container(
                    padding: EdgeInsets.only(bottom: 20.h, left: 10.w),
                    alignment: Alignment.bottomLeft,
                    child: FloatingActionButton(
                      elevation: 3,
                      onPressed: _delKey,
                      child: const Icon(Icons.logout),
                      tooltip: "Logout",
                    ),
                  ),
                  // Container(
                  //   padding: EdgeInsets.only(bottom: 20.h, left: 10.w),
                  //   alignment: Alignment.bottomRight,
                  //   child: FloatingActionButton(
                  //     elevation: 3,
                  //     onPressed: _notification,
                  //     child: const Icon(Icons.notifications),
                  //     tooltip: "Notification",
                  //   ),
                  // )
                ],
              ),
      ),
      // ],
    );
    // ),
  }
}
