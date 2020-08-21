# Simple Google API Intergration (SGAI)

## Table of Content
- [Enable Drive API](https://github.com/ZhakalenDk/Simple_Google_API_Intergration/blob/Development/README.md#enable-drive-api)
- [Quickstart Sample From Google](https://developers.google.com/drive/api/v3/quickstart/dotnet)
- [Authorizing Users with OAuth 2.0](https://github.com/ZhakalenDk/Simple_Google_API_Intergration/blob/Development/README.md#authorizing-users-with-oauth-20)
- [Import NuGet Packages](https://github.com/ZhakalenDk/Simple_Google_API_Intergration/blob/Development/README.md#import-nuget-packages)
- [Client ID & Client Secret](https://github.com/ZhakalenDk/Simple_Google_API_Intergration/blob/Development/README.md#client-id--client-secret)

### Enable Drive API
In order for you to [enable](https://developers.google.com/drive/api/v3/enable-drive-api) the [Drive API](https://developers.google.com/drive/api/v3/about-sdk) you will have to create a new project.
This is done by creating a new Cloud Platform for your application.
Go to [Google API Console](https://console.developers.google.com/) where you can create or select a project.
In the sidebar on the left you can expand **APIs & aouth** and select **APIs**.
This should show a list of available APIs. Click the **Drive API** link and click **Enable API**.
It's important that you, in resulting dialog, click **DOWNLOAD CLIENT CONFIGURATION** and **save** the file **credentials.json** to your **working** directory.

### Authorizing Users with OAuth 2.0
Every request your application sends to the [Drive API](https://developers.google.com/drive/api/v3/about-sdk) must include an authorization token.
This token is also used to identify your application to google.
To authorize an user you will have to do the following. (_This is a generel approach and might vary depending on the application you're writing_)
In the above step ([Enable Drive API](https://developers.google.com/drive/api/v3/enable-drive-api)) you've enabled the [Drive API](https://developers.google.com/drive/api/v3/about-sdk) by registering your application through the [Google API Console](https://console.developers.google.com/) in return you get a client ID and a client secret, which will be [used in a moment](https://github.com/ZhakalenDk/Simple_Google_API_Intergration/blob/Development/README.md#client-id--client-secret).
In order for your to access the users data you will need to define a **scope** of the kind of access you want. Google willthen display a screen where the user must give consent to authorize your application to the request of accessing the data your **scope** defines.

### Import NuGet Packages
Before you can use the SGAI library you will need to import the [Google API](https://developers.google.com/drive/api/v3/about-sdk) NuGet packages.
To install them you must execute
`PM> Install-Package Google.Apis.Drive.v3`

### Client ID & Client Secret
In [Enable Drive API](https://github.com/ZhakalenDk/Simple_Google_API_Intergration/blob/Development/README.md#enable-drive-api) you enabled your the API for your application. You then downloaded and saved a file called **credentials.json**.
This file contains your client ID and client secret. You should now include the file your in Visual Studio Solution.
When done, select the file and go to **Properties** and set the **Copy to Output Direectory** to **Copy Always**.
