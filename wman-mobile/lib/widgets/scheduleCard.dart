import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:intl/intl.dart';

class ScheduleCard extends StatelessWidget {
  final DateTime _estimatedStart;
  final DateTime _estimatedFinish;
  final DateTime _start;
  final DateTime _finish;
  ScheduleCard(
      this._estimatedFinish, this._estimatedStart, this._start, this._finish);
  @override
  Widget build(BuildContext context) {
    return Card(
      color: Colors.orangeAccent.withOpacity(0.5),
      child: Padding(
        padding: const EdgeInsets.all(8.0),
        child: Column(
          children: [
            RichText(
              textAlign: TextAlign.center,
              text: TextSpan(
                style: const TextStyle(
                    fontWeight: FontWeight.bold,
                    color: Colors.black,
                    fontSize: 20.0),
                text: "",
                children: <TextSpan>[
                  TextSpan(
                    text: 'Estimated\n ',
                    style: TextStyle(
                      fontWeight: FontWeight.normal,
                      fontSize: 20.w < 20
                          ? 20
                          : 20.w > 26
                              ? 26
                              : 20.w,
                      height: 1.5,
                    ),
                  ),
                  TextSpan(
                    text: DateFormat("yyyy.MM.dd, E")
                            .add_Hm()
                            .format(_estimatedStart) +
                        " - " +
                        DateFormat.Hm().format(_estimatedFinish),
                    style: TextStyle(
                        fontWeight: FontWeight.bold,
                        fontSize: 17.w < 17
                            ? 17
                            : 17.w > 23
                                ? 23
                                : 17.w),
                  ),
                  // --- spacer ----
                  TextSpan(
                    text: '\nDefinite\n',
                    style: TextStyle(
                      fontWeight: FontWeight.normal,
                      fontSize: 20.w < 20
                          ? 20
                          : 20.w > 26
                              ? 26
                              : 20.w,
                      height: 2,
                    ),
                  ),
                  TextSpan(
                    text: _start.year == 1
                        ? "Please start on schedule..."
                        // ignore: unrelated_type_equality_checks
                        : _finish.year == 1
                            ? DateFormat("yyyy.MM.dd, E")
                                    .add_Hm()
                                    .format(_start) +
                                " - " +
                                "(in progress)"
                            : DateFormat("yyyy.MM.dd, E")
                                    .add_Hm()
                                    .format(_start) +
                                " - " +
                                DateFormat.Hm().format(_finish),
                    style: TextStyle(
                      fontWeight: FontWeight.bold,
                      fontSize: 17.w < 17
                          ? 17
                          : 17.w > 23
                              ? 23
                              : 17.w,
                      height: 1.5,
                    ),
                  ),
                  // --- spacer ----
                ],
              ),
            ),
            // const Text(
            //   'Address',
            //   style: TextStyle(fontSize: 30),
            // ),
            // Text(
            //   'City: ${address.city}'
            //   "\nZIP Code:${address.zipCode}"
            //   "\nStreet: ${address.street} "
            //   "${address.buildingNumber}"
            //   "\nOptional: ${address.floorDoor}",
            // ),
          ],
        ),
      ),
    );
  }
}
