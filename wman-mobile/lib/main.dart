import 'dart:io';

import 'package:bot_toast/bot_toast.dart';
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:wman_mobile/widgets/login.dart';
import 'package:wman_mobile/widgets/workCard_detail.dart';
import 'package:wman_mobile/widgets/work_list.dart';
import 'package:http/http.dart' as http;

void main() {
  HttpOverrides.global = MyHttpOverrides();
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return ScreenUtilInit(
      builder: () => MaterialApp(
        title: 'WMAN Mobile',
        initialRoute: '/',
        builder: BotToastInit(),
        debugShowCheckedModeBanner: false,
        navigatorObservers: [BotToastNavigatorObserver()],
        theme: ThemeData(
          //primarySwatch: Colors.teal,
          primaryColor: Colors.teal,
          scaffoldBackgroundColor: Colors.teal[25],
          primarySwatch: Colors.teal,
          buttonTheme: ButtonThemeData(textTheme: ButtonTextTheme.accent),
          accentColor: Colors.teal[400],
          textTheme: TextTheme(
              //To support the following, you need to use the first initialization method
              ),
        ),
        routes: <String, WidgetBuilder>{
          '/': (context) => MyHomePage(),
          '/WorkList': (context) => WorkList(),
        },
      ),
    );
  }
}

class MyHomePage extends StatefulWidget {
  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  final storage = FlutterSecureStorage();
  String? token;
  Future<String?> getJwt() async {
    token = await storage.read(key: 'jwt');
    setState(() {});
    return token;
  }

  _MyHomePageState() {
    getJwt();
  }

  @override
  Widget build(BuildContext context) {
    return token == null ? Login() : WorkList();
    // return Login();
  }
}

class MyHttpOverrides extends HttpOverrides {
  @override
  HttpClient createHttpClient(SecurityContext? context) {
    return super.createHttpClient(context)
      ..badCertificateCallback =
          (X509Certificate cert, String host, int port) => true;
  }
}
