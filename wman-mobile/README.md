# WMAN Mobile
<p align="center">
<img src="./assets/wman_logo_teal.png" alt="wman logo" style="height: 200px; width: 200px;"/>
</p>

### Installation

It requires flutter. For installation do the following:
For Windows go here:

[https://docs.flutter.dev/get-started/install/windows](https://docs.flutter.dev/get-started/install/windows)

After the installation run the command:
`flutter doctor`
Make sure that all the dependencies are installed especially Android studio and the Android SDK.

### Connecting an Android Device

If you are using a physical Android device:
Running on localhost you need to port forward 5001 to 5001 using ADB.
For ADB go here:

[https://developer.android.com/studio/releases/platform-tools](https://developer.android.com/studio/releases/platform-tools)

To port forward run the following command:
`adb reverse tcp:5001 tcp:5001`

 Also make sure to enable USB debugging on your device in the Developer settings.

After connecting your device make sure it is available:
`flutter devices`

### Running the app
For running  use the following commands:

1. `flutter pub get`
2. `flutter run`

(you can use  the handler `-d` to select a certain device)
