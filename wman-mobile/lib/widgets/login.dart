import 'dart:convert';
import 'dart:io';

import 'package:flutter/services.dart';
import 'package:flutter_fadein/flutter_fadein.dart';
import 'package:bot_toast/bot_toast.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:jwt_decode/jwt_decode.dart';
import 'package:signalr_core/signalr_core.dart';
import 'package:wman_mobile/classes/Const/apiAcess.dart';
import 'package:wman_mobile/classes/signalR.dart';

class Login extends StatefulWidget {
  @override
  _LoginState createState() => _LoginState();
}

class _LoginState extends State<Login> {
  final _usernameController = TextEditingController();
  final _passwordController = TextEditingController();

  bool _isLoading = false;

  Future<void> _loginApiCall() async {
    print('Loading');
    setState(() {
      _isLoading = true;
    });
    print(_usernameController.text + ' ' + _passwordController.text);
    http.Response response = await http.put(
      Uri.parse(ApiAccess.baseUrl + "/Auth/login"),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
      },
      body: jsonEncode(<String, String>{
        'loginName': _usernameController.text,
        'password': _passwordController.text,
      }),
    );
    if (response.statusCode == 200) {
      Map<String, dynamic> responseBody = jsonDecode(response.body);
      Map<String, dynamic> payload = Jwt.parseJwt(responseBody['token']);
      print("EZT: " +
          payload[
              "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]);
      if (payload[
              "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] !=
          "Worker") {
        setState(() {
          _isLoading = false;
        });
        BotToast.showText(text: "You are not registered as a worker.");
        return;
      }
      await ApiAccess.secureStorage
          .write(key: 'jwt', value: responseBody['token']);
      SignalRAccess.signalr = SignalRService();
      SignalRAccess.signalr!.initSignalR();
      print("Connection is up: " +
          "${SignalRAccess.signalr!.hubConnection.state == HubConnectionState.connected}");
      await SignalRAccess.signalr!.hubConnection.start();
      print(responseBody['token']);
      setState(() {
        _isLoading = false;
      });
      Navigator.pushNamed(context, '/WorkList');
      return;
    }
    setState(() {
      _isLoading = false;
    });
    BotToast.showText(text: "Login failed");
  }

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: () async => false,
      child: Scaffold(
        appBar: AppBar(
          title: const Text('WMAN Mobile App'),
          automaticallyImplyLeading: false,
        ),
        body: Column(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          children: [
            _isLoading
                ? const Center(
                    child: CircularProgressIndicator(),
                  )
                : Flexible(
                    child: ScrollConfiguration(
                      behavior: DisableScrollGlow(),
                      child: ListView(
                        shrinkWrap: true,
                        children: [
                          Container(
                            margin: EdgeInsets.only(left: 30, right: 30),
                            child: Column(
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: [
                                // SizedBox(
                                //   width: 100.w,
                                //   height: 100.w,
                                //   child: const FadeInImage(
                                //     fadeInDuration: Duration(seconds: 1),
                                //     placeholder:
                                //         AssetImage('assets/wman_logo_white.png'),
                                //     image: AssetImage('assets/wman_logo_teal.png'),
                                //   ),
                                // ),
                                // placeholder: 'assets/wman_logo.png',
                                //     image: 'assets/wman_logo.png',
                                Padding(
                                  padding: EdgeInsets.only(
                                      top: 10.h < 5
                                          ? 5
                                          : 10.h > 20
                                              ? 20
                                              : 10.h,
                                      bottom: 10.h < 5
                                          ? 5
                                          : 10.h > 20
                                              ? 20
                                              : 10.h),
                                  child: Column(
                                    children: [
                                      FadeIn(
                                        child: SizedBox(
                                          width: 75.h < 5
                                              ? 5
                                              : 75.h > 100
                                                  ? 100
                                                  : 75.h,
                                          height: 75.h < 25
                                              ? 25
                                              : 75.h > 100
                                                  ? 100
                                                  : 75.h,
                                          child: Image(
                                            image: AssetImage(
                                                'assets/wman_logo_teal.png'),
                                          ),
                                        ),
                                        duration: Duration(seconds: 5),
                                        // Optional paramaters
                                        // duration: Duration(seconds: 3),
                                        curve: Curves.easeInCubic,
                                      ),
                                      FadeIn(
                                        child: Text(
                                          "WMAN Mobile",
                                          style: TextStyle(
                                              fontSize: 25.h < 5
                                                  ? 5
                                                  : 25.h > 35
                                                      ? 35
                                                      : 25.h),
                                        ),
                                        duration: Duration(seconds: 3),
                                        // Optional paramaters
                                        // duration: Duration(seconds: 3),
                                        curve: Curves.easeInExpo,
                                      ),
                                    ],
                                  ),
                                ),
                                Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      'Username',
                                      style: TextStyle(
                                          fontSize: 25.h < 5
                                              ? 5
                                              : 25.h > 28
                                                  ? 28
                                                  : 25.h),
                                    ),
                                    Padding(
                                      padding: EdgeInsets.only(
                                          top: 10.h < 5
                                              ? 5
                                              : 10.h > 20
                                                  ? 20
                                                  : 10.h,
                                          bottom: 10.h < 5
                                              ? 5
                                              : 10.h > 20
                                                  ? 20
                                                  : 10.h),
                                      child: TextField(
                                        controller: _usernameController,
                                        decoration: InputDecoration(
                                          hintText:
                                              'Enter your username here...',
                                          contentPadding: EdgeInsets.only(
                                              top: 10.h < 5
                                                  ? 5
                                                  : 10.h > 20
                                                      ? 20
                                                      : 10.h,
                                              bottom: 10.h < 5
                                                  ? 5
                                                  : 10.h > 20
                                                      ? 20
                                                      : 10.h),
                                        ),
                                      ),
                                    ),
                                    Text(
                                      'Password',
                                      style: TextStyle(
                                          fontSize: 25.h < 5
                                              ? 5
                                              : 25.h > 28
                                                  ? 28
                                                  : 25.h),
                                      textAlign: TextAlign.left,
                                    ),
                                    Padding(
                                      padding: EdgeInsets.only(
                                          top: 10.h < 10
                                              ? 10
                                              : 10.h > 20
                                                  ? 20
                                                  : 10.h,
                                          bottom: 10.h < 10
                                              ? 10
                                              : 10.h > 20
                                                  ? 20
                                                  : 10.h),
                                      child: TextField(
                                        controller: _passwordController,
                                        decoration: InputDecoration(
                                          hintText:
                                              'Enter your password here...',
                                          contentPadding: EdgeInsets.only(
                                              top: 10.h < 10
                                                  ? 10
                                                  : 10.h > 20
                                                      ? 20
                                                      : 10.h,
                                              bottom: 20.h < 10
                                                  ? 10
                                                  : 20.h > 20
                                                      ? 20
                                                      : 20.h),
                                        ),
                                        obscureText: true,
                                        enableSuggestions: false,
                                        autocorrect: false,
                                      ),
                                    ),
                                  ],
                                ),
                                Padding(
                                  padding: EdgeInsets.all(10),
                                  child: ElevatedButton(
                                      onPressed: _loginApiCall,
                                      child: const Text('Login')),
                                ),
                              ],
                            ),
                          )
                        ],
                      ),
                    ),
                  ),
          ],
        ),
      ),
    );
  }
}

class DisableScrollGlow extends ScrollBehavior {
  @override
  Widget buildViewportChrome(
      BuildContext context, Widget child, AxisDirection axisDirection) {
    return child;
  }
}
