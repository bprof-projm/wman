import 'dart:io';
import 'package:dio/dio.dart';

import 'package:bot_toast/bot_toast.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:image_picker/image_picker.dart';
import 'package:url_launcher/url_launcher.dart';
import 'package:http/http.dart' as http;
import 'package:wman_mobile/classes/Const/apiAcess.dart';
import 'package:wman_mobile/widgets/assigneduserList.dart';
import 'package:wman_mobile/widgets/labelListScroller.dart';
import 'package:wman_mobile/widgets/proofPictureScroller.dart';
import 'package:wman_mobile/widgets/scheduleCard.dart';
import '../classes/workEventCard.dart';
import 'addresCard.dart';

class WorkCardDetail extends StatefulWidget {
  final WorkEventCard workData;
  final ValueChanged<WorkEventCard> valueChanged;

  set workdata(WorkEventCard workdata) {}

  WorkCardDetail({Key? key, required this.workData, required this.valueChanged})
      : super(key: key);
  @override
  _WorkCardDetailState createState() =>
      _WorkCardDetailState(workData, valueChanged);
}

class _WorkCardDetailState extends State<WorkCardDetail> {
  ImagePicker _picker = ImagePicker();
  double _progression = 0;
  bool _isLoadingProgress = false;
  bool _isLoading = false;
  bool _deleteMode = false;
  _indicateProgress(int sent, total) {
    setState(() {
      // sleep(Duration(milliseconds: 100));
      _progression = ((sent / total * 100) / 100);
    });
  }

  _photoFromCamera() async {
    // Capture a photo
    XFile? photo = await _picker.pickImage(source: ImageSource.camera);
    setState(() {
      _progression = 0;
      _isLoadingProgress = true;
    });
    if (photo == null) {
      setState(() {
        _isLoadingProgress = false;
      });
      return;
    }
    String? token = await ApiAccess.secureStorage.read(key: 'jwt');

    Response<String> response;
    var dio = Dio();

    var formData = FormData.fromMap({
      'file': await MultipartFile.fromFile(photo.path, filename: photo.name),
    });
    response = await dio.post(
      ApiAccess.baseUrl + "/Photo/AddProofOfWorkPhoto/${_workdata.id}",
      data: formData,
      onSendProgress: _indicateProgress,
      options: Options(
        headers: <String, String>{
          'Content-Type': 'multipart/form-data',
          'Authorization': 'Bearer $token',
        },
      ),
    );
    if (response.statusCode == 200) {
      // print("SIKER");
      setState(() {
        // print(response.data.toString());
        _workdata.proofOfWorkPic =
            proofOfWorkListFromJson(response.data.toString());
      });
    }
    setState(() {
      _isLoadingProgress = false;
    });
  }

  _photoFromGalery() async {
    // Pick an image
    XFile? image = await _picker.pickImage(source: ImageSource.gallery);
    setState(() {
      _progression = 0;
      _isLoadingProgress = true;
    });
    if (image == null) {
      setState(() {
        _isLoadingProgress = false;
      });
      return;
    }
    String? token = await ApiAccess.secureStorage.read(key: 'jwt');

    Response<String> response;
    var dio = Dio();

    var formData = FormData.fromMap({
      'file': await MultipartFile.fromFile(image.path, filename: image.name),
    });
    response = await dio.post(
      ApiAccess.baseUrl + "/Photo/AddProofOfWorkPhoto/${_workdata.id}",
      data: formData,
      onSendProgress: _indicateProgress,
      options: Options(
        headers: <String, String>{
          'Content-Type': 'multipart/form-data',
          'Authorization': 'Bearer $token',
        },
      ),
    );
    if (response.statusCode == 200) {
      // print("SIKER");
      setState(() {
        // print(response.data.toString());
        _workdata.proofOfWorkPic =
            proofOfWorkListFromJson(response.data.toString());
      });
    }
    setState(() {
      _isLoadingProgress = false;
    });
  }

  WorkEventCard _workdata;
  final ValueChanged<WorkEventCard> _valueChanged;
  _WorkCardDetailState(this._workdata, this._valueChanged) {}
  Future<void> _modifyState() async {
    print('Loading');
    setState(() {
      _isLoading = true;
    });
    String? token = await ApiAccess.secureStorage.read(key: 'jwt');
    http.Response response = await http.put(
      Uri.parse(ApiAccess.baseUrl + "/StatusUpdater?eventID=${_workdata.id}"),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
        'Authorization': 'Bearer $token',
      },
    );
    if (response.statusCode == 200) {
      setState(() {
        _workdata = workEventFromJson(response.body);
        _isLoading = false;
        _valueChanged(_workdata);
      });
      return;
    } else if (response.statusCode == 409) {
      setState(() {
        _isLoading = false;
      });
      print(response.body);
      BotToast.showText(text: response.body);
      return;
    }
    print(response.statusCode);
    setState(() {
      _isLoading = false;
    });
    BotToast.showText(text: "State modify failed.");
  }

  void _proofPicturesChanged(List<ProofOfWorkPic> proofpics) {
    setState(() {
      _workdata.proofOfWorkPic = proofpics;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: FittedBox(child: Text(_workdata.jobDescription)),
      ),
      body: _isLoading
          ? const Center(
              child: CircularProgressIndicator(),
            )
          : _isLoadingProgress
              // : true
              ? Center(
                  child: Padding(
                  padding: const EdgeInsets.all(25.0),
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                    children: [
                      Text(
                        "Uploading image...\n" +
                            (_progression * 100).toStringAsFixed(0) +
                            "%",
                        style: TextStyle(
                          fontSize: 30.w < 10
                              ? 10
                              : 30.w > 41
                                  ? 41
                                  : 30.w,
                          color: Colors.grey,
                        ),
                        textAlign: TextAlign.center,
                      ),
                      LinearProgressIndicator(
                        value: _progression,
                        backgroundColor: Colors.grey,
                        minHeight: 20,
                      ),
                    ],
                  ),
                ))
              : Column(
                  children: [
                    Expanded(
                      child: ListView(
                        children: [
                          Column(
                            children: [
                              SizedBox(
                                width: double.infinity,
                                child: Card(
                                  color: Styling.statuscolor(_workdata.status),
                                  child: Padding(
                                    padding: const EdgeInsets.all(10.0),
                                    child: Column(
                                      mainAxisAlignment:
                                          MainAxisAlignment.center,
                                      children: [
                                        Text(
                                          _workdata.status[0].toUpperCase() +
                                              _workdata.status.substring(1),
                                          style: TextStyle(
                                            fontWeight: FontWeight.normal,
                                            fontSize: 35.w < 35
                                                ? 35
                                                : 35.w > 55
                                                    ? 55
                                                    : 35.w,
                                          ),
                                        ),
                                      ],
                                    ),
                                  ),
                                ),
                              ),
                              SizedBox(
                                width: double.infinity,
                                child: Card(
                                  child: Padding(
                                    padding: const EdgeInsets.all(10.0),
                                    child: Column(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                      mainAxisAlignment:
                                          MainAxisAlignment.spaceEvenly,
                                      children: [
                                        Text(
                                          'Schedule',
                                          style: TextStyle(
                                              fontWeight: FontWeight.bold,
                                              color: Colors.black,
                                              fontSize: 30.w < 30
                                                  ? 30
                                                  : 30.w > 40
                                                      ? 40
                                                      : 30.w),
                                        ),
                                        Divider(
                                            color:
                                                Colors.grey.withOpacity(0.5)),
                                        Center(
                                          child: ScheduleCard(
                                            _workdata.estimatedFinishDate,
                                            _workdata.estimatedStartDate,
                                            _workdata.workStartDate,
                                            _workdata.workFinishDate,
                                          ),
                                        ),
                                      ],
                                    ),
                                  ),
                                ),
                              ),
                              //----Spacer---///
                              SizedBox(
                                width: double.infinity,
                                child: Card(
                                  child: Padding(
                                    padding: const EdgeInsets.all(10.0),
                                    child: Column(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                      mainAxisAlignment:
                                          MainAxisAlignment.spaceEvenly,
                                      children: [
                                        Text(
                                          'Address',
                                          style: TextStyle(
                                              fontWeight: FontWeight.bold,
                                              color: Colors.black,
                                              fontSize: 30.w < 30
                                                  ? 30
                                                  : 30.w > 40
                                                      ? 40
                                                      : 30.w),
                                        ),
                                        Divider(
                                            color:
                                                Colors.grey.withOpacity(0.5)),
                                        Center(
                                            child:
                                                AddressCard(_workdata.address)),
                                      ],
                                    ),
                                  ),
                                ),
                              ),
                              //----Spacer---///
                              if (_workdata.labels.isNotEmpty)
                                SizedBox(
                                  width: double.infinity,
                                  child: Card(
                                    child: Padding(
                                      padding: const EdgeInsets.all(10.0),
                                      child: Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        mainAxisAlignment:
                                            MainAxisAlignment.spaceEvenly,
                                        children: [
                                          Text(
                                            'Labels',
                                            style: TextStyle(
                                                fontWeight: FontWeight.bold,
                                                color: Colors.black,
                                                fontSize: 30.w < 30
                                                    ? 30
                                                    : 30.w > 40
                                                        ? 40
                                                        : 30.w),
                                          ),
                                          Divider(
                                              color:
                                                  Colors.grey.withOpacity(0.5)),
                                          LabelListScroller(_workdata.labels)
                                        ],
                                      ),
                                    ),
                                  ),
                                ),
                              //----Spacer---///
                              SizedBox(
                                width: double.infinity,
                                height: 250.w < 250
                                    ? 250
                                    : 250.w > 300
                                        ? 300
                                        : 250.w,
                                child: Card(
                                  child: Padding(
                                    padding: const EdgeInsets.all(10.0),
                                    child: Column(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                      mainAxisAlignment:
                                          MainAxisAlignment.spaceEvenly,
                                      children: [
                                        Text(
                                          'Assigned users',
                                          style: TextStyle(
                                              fontWeight: FontWeight.bold,
                                              color: Colors.black,
                                              fontSize: 30.w < 30
                                                  ? 30
                                                  : 30.w > 40
                                                      ? 40
                                                      : 30.w),
                                        ),
                                        Divider(
                                            color:
                                                Colors.grey.withOpacity(0.5)),
                                        AssignedUserList(
                                            _workdata.assignedUsers,
                                            _workdata.jobDescription),
                                      ],
                                    ),
                                  ),
                                ),
                              ),
                              //----Spacer---///
                              if (_workdata.status == "proofawait" ||
                                  _workdata.status == "finished")
                                SizedBox(
                                  width: double.infinity,
                                  height: 300.w < 300
                                      ? 300
                                      : 300.w > 350
                                          ? 350
                                          : 300.w,
                                  child: Card(
                                    child: Padding(
                                      padding: const EdgeInsets.all(10.0),
                                      child: Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        mainAxisAlignment:
                                            MainAxisAlignment.spaceEvenly,
                                        children: [
                                          Row(
                                            children: [
                                              Text(
                                                'Proof of work pictures',
                                                style: TextStyle(
                                                    fontWeight: FontWeight.bold,
                                                    color: Colors.black,
                                                    fontSize: 30.w < 30
                                                        ? 30
                                                        : 30.w > 40
                                                            ? 40
                                                            : 30.w),
                                              ),
                                              if (_workdata.status ==
                                                      "proofawait" &&
                                                  _workdata.proofOfWorkPic
                                                      .isNotEmpty)
                                                Switch(
                                                  activeColor: Colors.red,
                                                  value: _deleteMode,
                                                  onChanged: (value) {
                                                    setState(() {
                                                      _deleteMode = value;
                                                    });
                                                  },
                                                ),
                                            ],
                                          ),
                                          Divider(
                                              color:
                                                  Colors.grey.withOpacity(0.5)),
                                          _workdata.proofOfWorkPic.isEmpty
                                              ? Center(
                                                  child: Padding(
                                                    padding:
                                                        const EdgeInsets.all(
                                                            20.0),
                                                    child: Text(
                                                      "Please provide proof of work photo or photos",
                                                      style: TextStyle(
                                                        fontSize: 30.w < 10
                                                            ? 10
                                                            : 30.w > 41
                                                                ? 41
                                                                : 30.w,
                                                        color: Colors.grey,
                                                      ),
                                                      textAlign:
                                                          TextAlign.center,
                                                    ),
                                                  ),
                                                )
                                              : ProofPictureScroller(
                                                  List.from(_workdata
                                                      .proofOfWorkPic.reversed),
                                                  _workdata.status ==
                                                          "proofawait"
                                                      ? _deleteMode
                                                      : false,
                                                  _proofPicturesChanged),
                                          if (_workdata.status == "proofawait")
                                            Row(
                                              crossAxisAlignment:
                                                  CrossAxisAlignment.center,
                                              mainAxisAlignment:
                                                  MainAxisAlignment.spaceEvenly,
                                              children: [
                                                ElevatedButton(
                                                  child: Row(
                                                    children: [
                                                      Icon(Icons.camera_alt),
                                                    ],
                                                  ),
                                                  onPressed: () async {
                                                    await _photoFromCamera();
                                                  },
                                                ),
                                                ElevatedButton(
                                                  child: Row(
                                                    children: [
                                                      Icon(Icons.image_rounded),
                                                    ],
                                                  ),
                                                  onPressed: () async {
                                                    await _photoFromGalery();
                                                  },
                                                ),
                                              ],
                                            ),
                                        ],
                                      ),
                                    ),
                                  ),
                                ),
                            ],
                          ),
                        ],
                      ),
                    ),
                    if (_workdata.status != "finished")
                      Padding(
                        padding: const EdgeInsets.all(13.0),
                        child: Container(
                          alignment: Alignment.bottomCenter,
                          child: SizedBox(
                            height: 80.w < 40
                                ? 40
                                : 80.w > 100
                                    ? 100
                                    : 80.w,
                            width: double.infinity,
                            child: ElevatedButton(
                              style: ElevatedButton.styleFrom(
                                primary: Styling.statuscolor(_workdata.status),
                                onPrimary: Colors.black,
                              ),
                              onPressed: () => showDialog(
                                context: context,
                                builder: (_) => AlertDialog(
                                  title: Text(
                                    "Modify state!",
                                    textAlign: TextAlign.center,
                                  ),
                                  content: Text(
                                    "This action is irreversible.\n Are you sure?",
                                    textAlign: TextAlign.center,
                                  ),
                                  actions: [
                                    Padding(
                                      padding: const EdgeInsets.all(8.0),
                                      child: Row(
                                        // crossAxisAlignment: CrossAxisAlignment.center,
                                        children: [
                                          ElevatedButton(
                                              child: Text("Yes"),
                                              onPressed: () async => {
                                                    await _modifyState(),
                                                    Navigator.pop(context),
                                                  }),
                                          const Flexible(
                                            fit: FlexFit.tight,
                                            child: SizedBox(),
                                          ),
                                          ElevatedButton(
                                            style: ElevatedButton.styleFrom(
                                                primary: Colors.red),
                                            child: Text("No"),
                                            onPressed: () {
                                              print(_workdata.id);
                                              Navigator.pop(context);
                                            },
                                          ),
                                        ],
                                      ),
                                    ),
                                  ],
                                ),
                                barrierDismissible: true,
                              ),
                              child: Text(
                                Styling.statusButtonText(_workdata.status),
                                style: TextStyle(
                                    fontSize: 28.w < 28
                                        ? 28
                                        : 28.w > 35
                                            ? 35
                                            : 28.w),
                              ),
                            ),
                          ),
                        ),
                      ),
                  ],
                ),
    );
  }
}
