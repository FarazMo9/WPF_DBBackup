# WPF_DBBackup

WPF Database Backup Application

The app is developed in order to prepare the SQL server and MySQL database backup files by their connection string and then encrypt the exported backup file. 
The process is executed on the user desired intervals which can be set on the app. 
Also by setting the FTP connection info, it will be uploaded to the FTP endpoint.

How it works ?
The app requests 16 characters key while running for the first time or whenever the key config files has been lost on working directory. 
Then the encryption config is ready and you need to set the app config and also add a database info. 
By setting the app config data and clicking on the "Start" button the process will be executed.

*If you already have the encryption key file, while initializing new instance of the app it can be imported to it. So the old backup files can be restored.*

The intervals are controlled by "System.Threading.Timers".
The encryption is generated by using AES.
The FTP connection process is managed by FluentFTP nuget package.
For the MySQL backup, the MySQLBackup.Net package has been used.
Database of the application is handled by SQLite which is integrated by the Entity Framework Core.

I tried to respect the clean architecture while developing. Please feel free to communicate and share ideas for improving it.

Hope you to enjoy using that.
