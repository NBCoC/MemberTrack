# MemberTrack
Member / Visitor tracking app for NBCoC

Step by step guide for running the app in debug mode:

Setup

1. In Microsoft SQL Server Management Studio, open a connection to localhost.
2. Create a new database by the name of 'MemberTrack'  (default database name in .\MemberTrack.WebApi\appsettings.Development.json )
3. Open .\MemberTrack.DbUti\MemberTrack.sql in SQL Server Management Studio and run it against the new database.
4. Open .\MemberTrack.sln in Visual Studio and build it.
5. Open command prompt and change directory to .\MemberTrack.DbUtil\bin\Debug\net452\win7-x64
6. Run the following command:
	MemberTrack.DbUtil.exe
   The default parameters will be used:
      Datasource: localhost
	  Catalog: Membertrack
7. Install Aurelia CLI (Instructions at http://aurelia.io/hub.html#/doc/article/aurelia/framework/latest/the-aurelia-cli/1).
	  Including NodeJS (at least v6.9.5) and a Git Client if needed.
8. Run command: npm install
	


Web API

1. Using Visual Studio, open MemberTrack.WebApi project
2. In the 'Standard' toolbar change the 'Debug Target' from 'IIS Express' to 'MemberTrack.WebApi'
3. Run the project. A console app should launch stating that the Web API is running on port 5000.
4. Navigate to http://localhost:5000/membertrack/api/user/contextuser to make sure its working...
   You should get JSON like the following:
	{"message":"No authentication handler is configured to handle the scheme: Automatic"}


Client

1. Using Visual Studio Code, use the File\Open Folder... menu item to open MemberTrack.Client project
2. Open command prompt and navigate to the root directory of the MemberTrack.Client project
3. Run command: au run --watch
   Toubleshooting:  au run returns "Invalid Command: run"
   http://aurelia.io/hub.html#/doc/article/aurelia/framework/latest/the-aurelia-cli/12
4. Navigate to http://localhost:9000
