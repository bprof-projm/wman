import 'package:flutter/gestures.dart';
import 'package:http/http.dart' as http;
import 'package:flutter/material.dart';
import 'package:flutter_fadein/flutter_fadein.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:wman_mobile/classes/Const/apiAcess.dart';
import 'package:wman_mobile/classes/workEventCard.dart';

class ProofPictureScroller extends StatefulWidget {
  @override
  final List<ProofOfWorkPic> _proofPictures;
  final bool _deleteMode;
  final ValueChanged<List<ProofOfWorkPic>> _proofChanged;
  ProofPictureScroller(
      this._proofPictures, this._deleteMode, this._proofChanged);
  @override
  ProofPictureScrollerState createState() {
    return ProofPictureScrollerState(_proofPictures, _proofChanged);
  }
}

class ProofPictureScrollerState extends State<ProofPictureScroller> {
  final ValueChanged<List<ProofOfWorkPic>> _proofChanged;

  List<ProofOfWorkPic> _proofPictures;
  ProofPictureScrollerState(this._proofPictures, this._proofChanged) {
    // print(_proofPictures.length);
  }

  bool _isLoading = false;
  Future<void> _deleteProofPic(int eventId, String photoId) async {
    print('Loading');
    setState(() {
      _isLoading = true;
    });
    String? token = await ApiAccess.secureStorage.read(key: 'jwt');
    http.Response response = await http.delete(
      //Uri.parse("https://127.0.0.1:5001/Worker/events/today"),
      Uri.parse(ApiAccess.baseUrl +
          "/Photo/RemoveProofOfWorkPhoto?eventId=$eventId&cloudCloudPhotoID=$photoId"),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
        'Authorization': 'Bearer $token',
      },
    );
    if (response.statusCode == 200) {
      // List<WorkerEventCard> responseBody = jsonDecode(response.body);
      setState(() {
        this
            ._proofPictures
            .removeWhere((element) => element.cloudPhotoId == photoId);
        print(_proofPictures.length);
        _proofChanged(_proofPictures);
        _isLoading = false;
      });
    } else if (response.statusCode == 401) {}
    print(response.statusCode);
    // print("GET BODY: " + response.body);
    setState(() {
      _isLoading = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return _isLoading
        ? const Center(
            child: CircularProgressIndicator(),
          )
        : Container(
            // height: 140,
            height: 140.w < 140
                ? 140
                : 140.w > 200
                    ? 200
                    : 140.w,
            padding: EdgeInsets.all(5),
            child: ListView.builder(
              scrollDirection: Axis.horizontal,
              padding: EdgeInsets.all(1),
              itemBuilder: (ctx, pictureIndex) {
                return Stack(
                  children: [
                    SizedBox(
                      height: 200.w < 200
                          ? 200
                          : 200.w > 300
                              ? 300
                              : 200.w,
                      width: 175.w < 175
                          ? 175
                          : 175.w > 275
                              ? 275
                              : 175.w,
                      child: DecoratedBox(
                        decoration: BoxDecoration(
                          image: DecorationImage(
                            image:
                                NetworkImage(_proofPictures[pictureIndex].url),
                            // colorFilter: ColorFilter.mode(
                            //     Colors.black.withOpacity(0.5), BlendMode.darken),
                          ),
                        ),
                      ),
                    ),
                    if (widget._deleteMode)
                      Container(
                        height: 40.w < 40
                            ? 40
                            : 40.w > 60
                                ? 60
                                : 40.w,
                        padding: EdgeInsets.only(left: 5),
                        child: FadeIn(
                          duration: Duration(seconds: 1),
                          curve: Curves.easeInExpo,
                          child: SizedBox(
                            height: 40.w < 40
                                ? 40
                                : 40.w > 60
                                    ? 60
                                    : 40.w,
                            width: 40.w < 40
                                ? 40
                                : 40.w > 60
                                    ? 60
                                    : 40.w,
                            child: FloatingActionButton(
                              backgroundColor: Colors.red,
                              elevation: 1,
                              child: Icon(
                                Icons.delete,
                                size: 25.w < 25
                                    ? 25
                                    : 25.w > 40
                                        ? 40
                                        : 25.w,
                              ),
                              onPressed: () async {
                                await _deleteProofPic(
                                    _proofPictures[pictureIndex].workEventId,
                                    _proofPictures[pictureIndex].cloudPhotoId);
                              },
                            ),
                          ),
                        ),
                      ),
                  ],
                );
                // return SizedBox(
                //   // height: 100.w < 100
                //   //     ? 100
                //   //     : 100.w > 200
                //   //         ? 200
                //   //         : 100.w,
                //   child: DecoratedBox(
                //     decoration: BoxDecoration(
                //       // borderRadius: BorderRadius.circular(20),
                //       // boxShadow: [
                //       //   BoxShadow(
                //       //     color: Colors.grey.withOpacity(0.5),
                //       //     spreadRadius: 1,
                //       //     blurRadius: 1,
                //       //     offset: Offset(2, 3),
                //       //   ),
                //       // ],
                //       image: DecorationImage(
                //         image: NetworkImage(_proofPictures[pictureIndex].url),
                //       ),
                //     ),
                //   ),
                // );
              },
              itemCount: _proofPictures.length,
            ),
          );
  }
}
