import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:wman_mobile/classes/signalR.dart';

class ApiAccess {
  static const int _port = 5001;
  static const String _url = "https://127.0.0.1";

  static FlutterSecureStorage secureStorage = FlutterSecureStorage();
  static String baseUrl = '$_url:$_port';
}

class SignalRAccess {
  static SignalRService? signalr;
}

class Styling {
  static Color statuscolor(String status) {
    return status == "awaiting"
        ? Color(int.parse("0xFF" +
            "#F7F774".substring(1))) //overkill but finally i can see colors
        : status == "started"
            ? Color(int.parse("0xFF" + "#6069F0".substring(1)))
            : status == "proofawait"
                ? Color(int.parse("0xFF" + "#E0AB75".substring(1)))
                : Color(int.parse("0xFF" + "#59F0A4".substring(1)));
  }

  static String statusButtonText(String status) {
    return status == "awaiting"
        ? "Start Work" //overkill but finally i can see colors
        : status == "started"
            ? "Stop Work"
            : status == "proofawait"
                ? "Finalize"
                : "Finished";
  }
}
