List of APIs
==

| Method |                                               URI                                               |      Request body       |         Return object          |           Role           | Description |
| :----: | :---------------------------------------------------------------------------------------------: | :---------------------: | :----------------------------: | :----------------------: | :---------: |
|  PUT   |                                           /Auth/login                                           |    `LoginViewModel`     |            `Token`             |            -             |     Provides the login feature.     |
|  GET   |                                            /Auth/all                                            |            -            |      `ICollection<User>`       |         `Admin`          |     Returns the list of every user.     |
|  GET   |                               /Auth/username?username=`{string}`                                |            -            |             `User`             |     `Admin, Manager`     |     Returns a user specified by their username.     |
|  GET   |                     /Auth/role/set?username=`{string}`&rolename=`{string}`                      |            -            |               -                |         `Admin`          |     Sets the role of a user and removes the previous roles they had.     |
|  GET   |                             /Auth/role/members?rolename=`{string}`                              |            -            |      `ICollection<User>`       |         `Admin`          |     Returns the list of users that are in the specified role.     |
|  GET   |                                            /Auth/db                                             |            -            |               -                |            -             |     Fills the database with test data.     |
|  GET   |                               /CalendarEvent/GetCurrentDayEvents                                |            -            |    `ICollection<WorkCard>`     | `Admin, Manager, Worker` |     Returns today's workevents as a list.     |
|  GET   |                               /CalendarEvent/GetCurrentWeekEvents                               |            -            |    `ICollection<WorkCard>`     | `Admin, Manager, Worker` |     Returns the actual week's workevents as a list.     |
|  GET   |                               /CalendarEvent/GetDayEvents/`{int}`                               |            -            |    `ICollection<WorkCard>`     | `Admin, Manager, Worker` |     Returns the specified day's workevents as a list. The `int` value should be the day number of the year.     |
|  GET   |                              /CalendarEvent/GetWeekEvents/`{int}`                               |            -            |    `ICollection<WorkCard>`     | `Admin, Manager, Worker` |     Returns the specified week's workevents as a list. The `int` value should be the week number of the year.     |
|  GET   |                          /CalendarEvent/GetDayEvents?time=`2021-12-23`                          |            -            |    `ICollection<WorkCard>`     | `Admin, Manager, Worker` |     Returns the specified day's workevents as a list. The `time` value could be any DateTime format.     |
|  GET   |      /CalendarEvent/GetWeekEvents?startEventDate=`2021-12-01`&finishEventDate=`2021-12-31`      |            -            |    `ICollection<WorkCard>`     | `Admin, Manager, Worker` |     Returns the list of the workevents between the specified time intervals. The `startEventDate` and `finishEventDate` values could be any DateTime format.     |
|  GET   |                                 /CalendarEvent/WorkCard/`{int}`                                 |            -            |           `WorkCard`           | `Admin, Manager, Worker` |     Returns every data of the specified workevent containing labels, workers, address etc. The `int` value should be the workevent's `ID`.     |
|  POST  |                                          /CreateEvent                                           |       `WorkEvent`       |               -                |        `Manager`         |     Creates a workevent declared in the request body.      |
| DELETE |                                      /DeleteEvent/`{int}`                                       |            -            |               -                |     `Admin, Manager`     |     Deletes a specified workevent. The `int` value should be the workevent's `ID`.     |
|  PUT   |                                        /DnDEvent/`{int}`                                        |        `DnDTime`        |               -                |     `Admin, Manager`     |     Modifies the specified workevent's `estimatedStartDate` and `estimatedFinishDate` according to the `DnDTime` values. The `int` value should be the workevent's `ID`.     |
|  POST  |                        /Event/assign?eventid=`{int}`&userName=`{string}`                        |            -            |               -                |     `Admin, Manager`     |     Assigns a specific user to a specific workevent. The `int` value should be the workevent's `ID` and the `string` value should be the user's username.     |
|  POST  |                                /Event/massAssign?eventid=`{int}`                                | `ICollection<username>` |               -                |     `Admin, Manager`     |     Assigns the users declared in the request body to a specific workevent. The `int` value should be the workevent's `ID`.     |
|  PUT   |                                          /UpdateEvent                                           |    `WorkEventWithID`    |               -                |     `Admin, Manager`     |     Modifies the workevent declared in the request body.     |
|  POST  |                                          /CreateLabel                                           |       `NewLabel`        |               -                |        `Manager`         |     Creates a label declared in the request body.     |
|  GET   |                                          /GetAllLabel                                           |            -            |      `ICollection<Label>`      |        `Manager`         |     Returns every label as a list.     |
|  PUT   |                                      /UpdateLabel/`{int}`                                       |       `NewLabel`        |               -                |        `Manager`         |     Modifies the label declared in the request body. The `int` value should be the label's `ID`.     |
|  POST  |                     /AssignLabelToWorkEvent?eventId=`{int}`&labelId=`{int}`                     |            -            |               -                |        `Manager`         |     Assigns a specific label to a specific workevent. The first `int` value should be the workevent's `ID` and the second `int` value should be the label's `ID`.     |
| DELETE |                                      /DeleteLabel/`{int}`                                       |            -            |               -                |        `Manager`         |     Deletes a specified label. The `int` value should be the label's `ID`.     |
|  POST  |                               /Photo/AddProfilePhoto/`{string}`                               |       Image file        |            `Photo`             | `Admin, Manager, Worker` |     Adds a profile picture to the specified user. The `string` value should be the user's username.     |
| DELETE |                             /Photo/RemoveProfilePhoto/`{string}`                              |            -            |               -                | `Admin, Manager, Worker` |     Removes the profile picture of the specified user. The `string` value should be the user's username.     |
|  PUT   |                             /Photo/UpdateProfilePhoto/`{string}`                              |       Image file        |            `Photo`             | `Admin, Manager, Worker` |     Modifies the profile picture of the specified user. The `string` value should be the user's username.     |
|  GET   | /User/workload/range?usernames=`{string}`&usernames=`{string}`&usernames=`{string}` |            -            |    `ICollection<Workload>`     |     `Admin, Manager`     |     Returns the list of workloads related to the specified users. The `string` values should be the usernames of the users.      |
|  GET   |                                         /User/workload                                          |            -            |    `ICollection<Workload>`     |     `Admin, Manager`     |     Returns the list of workloads of every user.     |
|  GET   |                             /User/workEvents?username=`{string}`                              |            -            | `ICollection<WorkEventWithID>` |     `Admin, Manager`     |     Returns every workevent as a list that assigned to the specified user. The `string` value should be the user's username.      |
|  GET   |           /GetWorkersAvailablityAtSpecTime?fromDate=`2021-12-28`&toDate=`2021-12-31`            |            -            |      `ICollection<User>`       | `Admin, Manager, Worker` |     Returns the users (workers) as a list that available between the specified dates. The `fromDate` and `toDate` values could be any DateTime format.      |
&nbsp;

Models
==

`LoginViewModel` as JSON
```json
{
    "loginName": "string",
    "password": "string"
}
```

`Token` as JSON
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJVU0VSIiwianRpIjoiOTdhNDZhYjQtYWQxMi00ZGExLThlZTktM2JjMDQyZDhlNmIwIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI2IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IlVTRVIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJXb3JrZXIiLCJleHAiOjE2NDAzNDQwMzEsImlzcyI6Imh0dHA6Ly93d3cuc2VjdXJpdHkub3JnIiwiYXVkIjoiaHR0cDovL3d3dy5zZWN1cml0eS5vcmcifQ.7sKX-SURiDckjAdxeBOB6PQrUVEOZhlPiIisrmHns7Y",
  "expirationDate": "2021-12-24T11:07:11Z"
}
```

`User` as JSON
```json
{
    "username": "string",
    "email": "user@example.com",
    "firstname": "string",
    "lastname": "string",
    "picture": {
        "cloudPhotoID": "string",
        "url": "string",
        "wManUserID": 0
    }
}
```

`WorkCard` as JSON
```json
[
  {
    "id": 0,
    "jobDescription": "string",
    "estimatedStartDate": "2021-12-24T10:25:25.251Z",
    "estimatedFinishDate": "2021-12-24T11:25:25.251Z",
    "assignedUsers": [
      {
        "username": "string",
        "email": "user@example.com",
        "firstname": "string",
        "lastname": "string",
        "profilePicture": {
          "cloudPhotoID": "string",
          "url": "string",
          "wManUserID": 0
        }
      }
    ],
    "labels": [
      {
        "id": 0,
        "backgroundColor": "#FFFFFF",
        "textColor": "#000000",
        "content": "string"
      }
    ],
    "address": {
      "city": "string",
      "street": "string",
      "zipCode": 1111,
      "buildingNumber": "string",
      "floorDoor": "string"
    },
    "workStartDate": "2021-12-24T10:25:25.251Z",
    "workFinishDate": "2021-12-24T11:25:25.251Z",
    "status": "awaiting"
  }
]
```

`WorkEvent` as JSON
```json
{
    "jobDescription": "string",
    "estimatedStartDate": "2021-12-24T11:02:21.416Z",
    "estimatedFinishDate": "2021-12-24T13:02:21.416Z",
    "address": {
        "city": "string",
        "street": "string",
        "zipCode": 1111,
        "buildingNumber": "string",
        "floorDoor": "string"
    },
    "status": "awaiting"
}
```

`DnDTime` as JSON
```json
{
    "estimatedStartDate": "2021-12-24T11:09:37.125Z",
    "estimatedFinishDate": "2021-12-24T12:09:37.125Z"
}
```

`WorkEventWithID` as JSON
```json
{
    "id": 0,
    "jobDescription": "string",
    "estimatedStartDate": "2021-12-24T11:02:21.416Z",
    "estimatedFinishDate": "2021-12-24T13:02:21.416Z",
    "address": {
        "city": "string",
        "street": "string",
        "zipCode": 1111,
        "buildingNumber": "string",
        "floorDoor": "string"
    },
    "status": "awaiting"
}
```

`NewLabel` as JSON
```json
{
    "color": "#FFFFFF",
    "content": "string"
}
```

`Label` as JSON
```json
{
    "id": 0,
    "backgroundColor": "#FFFFFF",
    "textColor": "#000000",
    "content": "string"
}
```

`Photo` as JSON
```json
{
    "cloudPhotoID": "string",
    "url": "string",
    "wManUserID": 0
}
```

`Workload` as JSON
```json
{
    "userID": 0,
    "username": "string",
    "profilePicUrl": "string",
    "percent": 0
}
```