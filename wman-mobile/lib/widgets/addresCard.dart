import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:url_launcher/url_launcher.dart';
import 'package:wman_mobile/classes/workEventCard.dart';

class AddressCard extends StatelessWidget {
  final Address address;
  AddressCard(this.address);
  void _viewOnMap() async {
    String _url =
        'https://www.google.hu/maps/place/${address.city.replaceAll(' ', '+')}+${address.street.replaceAll(' ', '+')}+${address.buildingNumber.replaceAll(' ', '+')}+${address.zipCode.toString().replaceAll(' ', '+')}';
    print(_url);
    if (!await launch(_url)) throw 'Could not launch $_url';
  }

  @override
  Widget build(BuildContext context) {
    TextStyle propertyTextStyle = TextStyle(
      fontWeight: FontWeight.normal,
      fontSize: 18.w < 18
          ? 18
          : 18.w > 24
              ? 24
              : 18.w,
    );
    TextStyle valueTextStyle = TextStyle(
        fontWeight: FontWeight.bold,
        fontSize: 17.w < 17
            ? 17
            : 17.w > 23
                ? 23
                : 17.w);
    return Card(
      color: Colors.lightBlue.withOpacity(0.5),
      child: Padding(
        padding: const EdgeInsets.all(8.0),
        child: Column(
          children: [
            RichText(
              textAlign: TextAlign.justify,
              text: TextSpan(
                style: const TextStyle(
                    fontWeight: FontWeight.bold,
                    color: Colors.black,
                    fontSize: 20.0),
                text: "",
                children: <TextSpan>[
                  TextSpan(
                    text: 'City: ',
                    style: TextStyle(
                      fontWeight: FontWeight.normal,
                      fontSize: 18.w < 18
                          ? 18
                          : 18.w > 24
                              ? 24
                              : 18.w,
                    ),
                  ),
                  TextSpan(
                    text: address.city,
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
                    text: '\nZIP Code: ',
                    style: TextStyle(
                      fontWeight: FontWeight.normal,
                      fontSize: 18.w < 18
                          ? 18
                          : 18.w > 24
                              ? 24
                              : 18.w,
                    ),
                  ),
                  TextSpan(
                    text: address.zipCode.toString(),
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
                    text: '\nStreet: ',
                    style: TextStyle(
                      fontWeight: FontWeight.normal,
                      fontSize: 18.w < 18
                          ? 18
                          : 18.w > 24
                              ? 24
                              : 18.w,
                    ),
                  ),
                  TextSpan(
                    text: address.street + ' ',
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
                    text: address.buildingNumber,
                    style: TextStyle(
                        fontWeight: FontWeight.bold,
                        fontSize: 17.w < 17
                            ? 17
                            : 17.w > 23
                                ? 23
                                : 17.w),
                  ),
                  // --- spacer ----
                  if (address.floorDoor != null)
                    TextSpan(
                      text: '\nOptional: ',
                      style: TextStyle(
                        fontWeight: FontWeight.normal,
                        fontSize: 18.w < 18
                            ? 18
                            : 18.w > 24
                                ? 24
                                : 18.w,
                      ),
                    ),
                  if (address.floorDoor != null)
                    TextSpan(
                      text: address.floorDoor.toString(),
                      style: TextStyle(
                          fontWeight: FontWeight.bold,
                          fontSize: 17.w < 17
                              ? 17
                              : 17.w > 23
                                  ? 23
                                  : 17.w),
                    ),
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
            Padding(
              padding: const EdgeInsets.only(top: 10),
              child: ElevatedButton(
                onPressed: _viewOnMap,
                child: const Text('Go to location'),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
