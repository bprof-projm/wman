import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:wman_mobile/classes/workEventCard.dart';

class LabelListScroller extends StatelessWidget {
  final List<Label> labelList;
  LabelListScroller(this.labelList);
  @override
  Widget build(BuildContext context) {
    return Container(
      height: 50.w < 50
          ? 50
          : 50.w > 60
              ? 60
              : 50.w,
      width: double.infinity,
      // padding: EdgeInsets.all(5.w),
      padding: EdgeInsets.only(left: 10, right: 10, bottom: 5, top: 5),
      child: ListView.builder(
        scrollDirection: Axis.horizontal,
        padding: EdgeInsets.all(1),
        itemBuilder: (ctx, labelIndex) {
          return Padding(
            padding: EdgeInsets.only(left: 10, right: 10, bottom: 5, top: 5),
            child: SizedBox(
              child: DecoratedBox(
                decoration: BoxDecoration(
                  borderRadius: BorderRadius.circular(20),
                  boxShadow: [
                    BoxShadow(
                      color: Colors.grey.withOpacity(0.5),
                      spreadRadius: 1,
                      blurRadius: 1,
                      offset: Offset(2, 3),
                    ),
                  ],
                  color: Color(
                    int.parse("0xff" +
                        labelList[labelIndex].backgroundColor.substring(
                            1, labelList[labelIndex].backgroundColor.length)),
                  ),
                ),
                child: Center(
                  child: Padding(
                    padding: EdgeInsets.only(left: 8, right: 8),
                    child: Text(
                      labelList[labelIndex].content,
                      style: TextStyle(
                        fontSize: 13.w < 13
                            ? 13
                            : 13.w > 18
                                ? 18
                                : 13.w,
                        color: Color(
                          int.parse("0xff" +
                              labelList[labelIndex].textColor.substring(
                                  1, labelList[labelIndex].textColor.length)),
                        ),
                      ),
                    ),
                  ),
                ),
              ),
            ),
          );
        },
        itemCount: labelList.length,
      ),
    );
  }
}
