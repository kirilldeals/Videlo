# Videlo
### Description
The project is a video hosting website developed using ASP.NET Core. It allows users to upload, search for videos and leave feedback by writing comments and liking/disliking.
### Key features
- AWS S3 is used as storage for video and image files.
- Database management system: Microsoft SQL Server.
- Responsive design with Bootstrap 5 framework.
- JavaScript libraries: jQuery. AJAX is implemented for asynchronous requests with the server.
### Getting started
Configure the database connection string and AWS S3 settings in the `appsettings.json` file:
```json
  // YOUR DB CONNECTION
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  
  // YOUR AWS S3 CONFIGURATION
  "AWSConfiguration": {
    "ServiceEndpoint": "",
    "BucketName": "",
    "AccessKey": "",
    "SecretKey": ""
  }
```
Optionally, you can add a default user with an admin role. This user will be added to the database when the application starts:
```json
  "AdminCredentialsSettings": {
    "Username": "",
    "Email": "",
    "Password": ""
  }
```
### Demo
https://videlo.bsite.net *

__* video uploading unavailable for new users__
