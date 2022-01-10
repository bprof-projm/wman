// To parse this JSON data, do
//
//     final workEventCard = workEventCardFromJson(jsonString);

import 'dart:convert';

List<WorkEventCard> workEventCardFromJson(String str) =>
    List<WorkEventCard>.from(
        json.decode(str).map((x) => WorkEventCard.fromJson(x)));

String workEventCardToJson(List<WorkEventCard> data) =>
    json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

WorkEventCard workEventFromJson(String str) =>
    WorkEventCard.fromJson(json.decode(str));

List<ProofOfWorkPic> proofOfWorkListFromJson(String str) =>
    List<ProofOfWorkPic>.from(
        json.decode(str).map((x) => ProofOfWorkPic.fromJson(x)));

class WorkEventCard {
  WorkEventCard({
    required this.id,
    required this.jobDescription,
    required this.estimatedStartDate,
    required this.estimatedFinishDate,
    required this.assignedUsers,
    required this.labels,
    required this.address,
    required this.workStartDate,
    required this.workFinishDate,
    required this.proofOfWorkPic,
    required this.status,
  });

  int id;
  String jobDescription;
  DateTime estimatedStartDate;
  DateTime estimatedFinishDate;
  List<AssignedUser> assignedUsers;
  List<Label> labels;
  Address address;
  DateTime workStartDate;
  DateTime workFinishDate;
  List<ProofOfWorkPic> proofOfWorkPic;
  String status;

  factory WorkEventCard.fromJson(Map<String, dynamic> json) => WorkEventCard(
        id: json["id"],
        jobDescription: json["jobDescription"],
        estimatedStartDate: DateTime.parse(json["estimatedStartDate"]),
        estimatedFinishDate: DateTime.parse(json["estimatedFinishDate"]),
        assignedUsers: List<AssignedUser>.from(
            json["assignedUsers"].map((x) => AssignedUser.fromJson(x))),
        labels: List<Label>.from(json["labels"].map((x) => Label.fromJson(x))),
        address: Address.fromJson(json["address"]),
        workStartDate: DateTime.parse(json["workStartDate"]),
        workFinishDate: DateTime.parse(json["workFinishDate"]),
        proofOfWorkPic: List<ProofOfWorkPic>.from(
            json["proofOfWorkPic"].map((x) => ProofOfWorkPic.fromJson(x))),
        status: json["status"],
      );

  Map<String, dynamic> toJson() => {
        "id": id,
        "jobDescription": jobDescription,
        "estimatedStartDate": estimatedStartDate.toIso8601String(),
        "estimatedFinishDate": estimatedFinishDate.toIso8601String(),
        "assignedUsers":
            List<dynamic>.from(assignedUsers.map((x) => x.toJson())),
        "labels": List<dynamic>.from(labels.map((x) => x.toJson())),
        "address": address.toJson(),
        "workStartDate": workStartDate.toIso8601String(),
        "workFinishDate": workFinishDate.toIso8601String(),
        "proofOfWorkPic":
            List<dynamic>.from(proofOfWorkPic.map((x) => x.toJson())),
        "status": status,
      };
}

class Address {
  Address({
    required this.city,
    required this.street,
    required this.zipCode,
    required this.buildingNumber,
    required this.floorDoor,
  });

  String city;
  String street;
  int zipCode;
  String buildingNumber;
  String? floorDoor;

  factory Address.fromJson(Map<String, dynamic> json) => Address(
        city: json["city"],
        street: json["street"],
        zipCode: json["zipCode"],
        buildingNumber: json["buildingNumber"],
        floorDoor: json["floorDoor"],
      );

  Map<String, dynamic> toJson() => {
        "city": city,
        "street": street,
        "zipCode": zipCode,
        "buildingNumber": buildingNumber,
        "floorDoor": floorDoor,
      };
}

class AssignedUser {
  AssignedUser({
    required this.username,
    required this.email,
    required this.firstname,
    required this.lastname,
    required this.profilePicture,
    required this.phoneNumber,
  });

  String username;
  String email;
  String firstname;
  String lastname;
  String phoneNumber;
  ProfilePicture? profilePicture;

  factory AssignedUser.fromJson(Map<String, dynamic> json) => AssignedUser(
        username: json["username"],
        email: json["email"],
        firstname: json["firstname"],
        lastname: json["lastname"],
        phoneNumber: json["phoneNumber"],
        profilePicture: json["profilePicture"] == null
            ? null
            : ProfilePicture.fromJson(json["profilePicture"]),
      );

  Map<String, dynamic> toJson() => {
        "username": username,
        "email": email,
        "firstname": firstname,
        "lastname": lastname,
        "phoneNumber": phoneNumber,
        "profilePicture": profilePicture?.toJson(),
      };
}

class ProfilePicture {
  ProfilePicture({
    required this.cloudPhotoId,
    required this.url,
    required this.wManUserId,
  });

  dynamic cloudPhotoId;
  String url;
  int wManUserId;

  factory ProfilePicture.fromJson(Map<String, dynamic> json) => ProfilePicture(
        cloudPhotoId: json["cloudPhotoID"],
        url: json["url"],
        wManUserId: json["wManUserID"],
      );

  Map<String, dynamic> toJson() => {
        "cloudPhotoID": cloudPhotoId,
        "url": url,
        "wManUserID": wManUserId,
      };
}

class Label {
  Label({
    required this.id,
    required this.backgroundColor,
    required this.textColor,
    required this.content,
  });

  int id;
  String backgroundColor;
  String textColor;
  String content;

  factory Label.fromJson(Map<String, dynamic> json) => Label(
        id: json["id"],
        backgroundColor: json["backgroundColor"],
        textColor: json["textColor"],
        content: json["content"],
      );

  Map<String, dynamic> toJson() => {
        "id": id,
        "backgroundColor": backgroundColor,
        "textColor": textColor,
        "content": content,
      };
}

class ProofOfWorkPic {
  ProofOfWorkPic({
    required this.cloudPhotoId,
    required this.url,
    required this.workEventId,
  });

  String cloudPhotoId;
  String url;
  int workEventId;

  factory ProofOfWorkPic.fromJson(Map<String, dynamic> json) => ProofOfWorkPic(
        cloudPhotoId: json["cloudPhotoID"],
        url: json["url"],
        workEventId: json["workEventID"],
      );

  Map<String, dynamic> toJson() => {
        "cloudPhotoID": cloudPhotoId,
        "url": url,
        "workEventID": workEventId,
      };
}
