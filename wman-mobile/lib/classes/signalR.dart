import 'package:signalr_core/signalr_core.dart';
import 'Const/apiAcess.dart';

class SignalRService {
  final hubUrl = ApiAccess.baseUrl + "/notify";
  late HubConnection hubConnection;
  SignalRService() {
    initSignalR();
  }

  Future<String?> getAccessToken() async {
    var v = await ApiAccess.secureStorage.read(key: "jwt");
    return v;
  }

  void initSignalR() {
    // Init SignalR
    // hubConnection = HubConnectionBuilder().withUrl(hubUrl).build();
    hubConnection = HubConnectionBuilder()
        .withUrl(
            hubUrl,
            HttpConnectionOptions(
              accessTokenFactory: () async => await getAccessToken(),
              withCredentials: true,
              transport: HttpTransportType.webSockets,
            ))
        .build();
    hubConnection.serverTimeoutInMilliseconds = 100000;
    // hubConnection = HubConnectionBuilder().withUrl(hubUrl).build();
    hubConnection.onclose((error) => print("SignalR connection closed!."));
    print("Connection is up: " +
        "${hubConnection.state == HubConnectionState.connected}");

    // hubConnection.on("UserAssiged", _userAssigned);
    // hubConnection.on("UserAssiged", (arguments) {
    //   print(arguments);
    //   BotToast.showSimpleNotification(
    //       title: "Rá lettél rakva egy eventre",
    //       duration: Duration(seconds: 10));
    // });
  }
}
