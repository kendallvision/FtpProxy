# FtpProxy

## Technologies Included
* .NetCore 2.2
* WebAPI
* NLog

## Build for Publish
* Open solution in Visual Studio (primary development done using Visual Studio 2019 Community Edition)
* Build the solution in Release
* Publish the solution to file

This will result in a "publish" directory with the files you should need for deployment.  The project is meant to be hosted in IIS on a server that supports .NetCore 2.2.

## Setup
The following settings in the **appSettings.json** file need to be set before live usage:

#### FtpServerAddress
The base address of the FTP server.  For example: "ftp://ftp.dlptest.com"

#### DropFolder
The folder on the FTP server to which to drop files sent from EZPrints to external partner.  For example: "dropfolder"

#### PickupFolder
The folder on the FTP where completed files will be placed for pickup by EZPrints.  The proxy service will scan this folder for files to download.  For example: "pickupFolder"

#### FtpUser
The user name to use for FTP username to access the FTP server.

#### FtpPassword
The password to use for the FTP password to access the FTP server.

#### FrequencyToCheckFilesInSeconds
The number of seconds to delay between scans of the **PickupFolder**.  For example: "300"

#### LocalDestination
The full path to the local destination folder for completed files.  Files found in the **PickupFolder** are downloaded to this directory.  For example: "\\serverName\localDropFolder"

#### NotificationAddress
The full address the Proxy will use to POST a notification to.  For example: "http://myserver:9000/api/notify".  

## Proxy API Usage

### /api/proxy GET
If you send a GET request to the /api/proxy it should respond with an "alive" message.  A good way to do this is to navigate to that adress in a browser.

### /api/proxy POST
To send a file to the partner send a POST request to **/api/proxy**

### notifications
When a file is downloaded from the FTP server to the local destination, a notificatoin POST will be sent to the configured API. The POST will include a source element specifying the full filename of the file associated with the notification event.  The API catching the notification POST should extract the filename the URI and it can then copy the file for further processing.

## Logging
Logging is accomplished using NLog.  The nlog.config file can be used to add additional configuration and customization of the logging for the service.  For more information see https://github.com/NLog/NLog/wiki/Configuration-file
