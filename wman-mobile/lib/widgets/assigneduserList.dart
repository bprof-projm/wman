import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:url_launcher/url_launcher.dart';
import 'package:wman_mobile/classes/workEventCard.dart';

class AssignedUserList extends StatelessWidget {
  final List<AssignedUser> assignedUsers;
  final String _jobDescription;
  AssignedUserList(this.assignedUsers, this._jobDescription);
  String? encodeQueryParameters(Map<String, String> params) {
    return params.entries
        .map((e) =>
            '${Uri.encodeComponent(e.key)}=${Uri.encodeComponent(e.value)}')
        .join('&');
  }

  void _sendMail(String emailAddress, String exampleSubject) {
    Uri emailLaunchUri = Uri(
        scheme: 'mailto',
        path: emailAddress,
        query: encodeQueryParameters(
            <String, String>{'subject': 'ABOUT: $exampleSubject'}));
    launch(emailLaunchUri.toString());
  }

  void _sendSms(String phoneNumber) {
    Uri emailLaunchUri = Uri(
      scheme: 'sms',
      path: phoneNumber,
    );
    launch(emailLaunchUri.toString());
  }

  void _callPhoneNumber(String phoneNumber) {
    Uri emailLaunchUri = Uri(
      scheme: 'tel',
      path: phoneNumber,
    );
    launch(emailLaunchUri.toString());
  }

  @override
  Widget build(BuildContext context) {
    TextStyle propertyTextStyle = TextStyle(
        fontWeight: FontWeight.normal,
        fontSize: 18.w < 40
            ? 8
            : 18.w > 210
                ? 21
                : 18.w);
    TextStyle valueTextStyle = TextStyle(
      fontWeight: FontWeight.bold,
      fontSize: 17.w < 13
          ? 13
          : 17.w > 18
              ? 20
              : 17.w,
    );
    double _iconBoxSize() {
      return 40.w < 35
          ? 35
          : 40.w > 60
              ? 60
              : 40.w;
    }

    double _iconSize() {
      return 25.w < 25
          ? 25
          : 25.w > 30
              ? 30
              : 25.w;
    }

    return Expanded(
      child: ListView.builder(
        itemBuilder: (ctx, userIndex) {
          return Card(
            color: Colors.lightBlue.withOpacity(0.5),
            child: Container(
              height: 80,
              child: Padding(
                padding: const EdgeInsets.all(2),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                  // crossAxisAlignment: CrossAxisAlignment.start,
                  //mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    SizedBox(
                      width: 50.w < 50
                          ? 50
                          : 50.w > 100
                              ? 100
                              : 50.w,
                      height: 50.w < 50
                          ? 50
                          : 50.w > 100
                              ? 100
                              : 50.w,
                      child: DecoratedBox(
                        decoration: BoxDecoration(
                          shape: BoxShape.circle,
                          border: Border.all(
                              color: Colors.black,
                              width: 1.0,
                              style: BorderStyle.solid),
                          image: DecorationImage(
                            image: assignedUsers[userIndex].profilePicture ==
                                    null
                                ? const AssetImage(
                                        "assets/wman_profpic_placeholder.png")
                                    as ImageProvider
                                : NetworkImage(assignedUsers[userIndex]
                                    .profilePicture!
                                    .url),
                            // NetworkImage(assignedUsers[userIndex] .profilePicture.url),
                          ),
                        ),
                      ),
                    ),
                    // const Flexible(
                    //   fit: FlexFit.tight,
                    //   child: SizedBox(),
                    // ),
                    Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        SizedBox(
                          width: 100.w < 50
                              ? 50
                              : 100.w > 200
                                  ? 200
                                  : 100.w,
                          child: Text(
                            assignedUsers[userIndex].firstname +
                                '\n' +
                                assignedUsers[userIndex].lastname,
                            // "Alessandro\nBlaineWolfeschlegelsteinhausenbergerdorffaaaaaaaaaaaa",
                            style: valueTextStyle,
                            textAlign: TextAlign.center,
                            overflow: TextOverflow.ellipsis,
                          ),
                        ),
                      ],
                    ),
                    // --- spacer ----
                    Row(
                      children: [
                        SizedBox(
                          width: _iconBoxSize(),
                          child: RawMaterialButton(
                            onPressed: () => _sendMail(
                                assignedUsers[userIndex].email,
                                _jobDescription),
                            fillColor: Colors.red,
                            child: Icon(
                              Icons.email,
                              size: _iconSize(),
                              color: Colors.white,
                            ),
                            shape: CircleBorder(),
                          ),
                        ),
                        SizedBox(
                          width: _iconBoxSize(),
                          child: RawMaterialButton(
                            onPressed: () =>
                                _sendSms(assignedUsers[userIndex].phoneNumber),
                            fillColor: Colors.green,
                            child: Icon(
                              Icons.sms,
                              size: _iconSize(),
                              color: Colors.white,
                            ),
                            shape: CircleBorder(),
                          ),
                        ),
                        SizedBox(
                          width: _iconBoxSize(),
                          child: RawMaterialButton(
                            onPressed: () => _callPhoneNumber(
                                assignedUsers[userIndex].phoneNumber),
                            fillColor: Colors.blue,
                            child: Icon(
                              Icons.phone,
                              size: _iconSize(),
                              color: Colors.white,
                            ),
                            shape: CircleBorder(),
                          ),
                        ),
                      ],
                    ),

                    // const Flexible(
                    //   fit: FlexFit.tight,
                    //   child: SizedBox(),
                    // ),
                  ],
                ),
              ),
            ),
          );
        },
        itemCount: assignedUsers.length,
      ),
    );
  }
}
