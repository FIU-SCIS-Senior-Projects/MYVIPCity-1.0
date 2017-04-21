
# Project structure

## Solution
The solution that contains all the projects for MyVIPCity 1.0 is **MyVipCity.sln**. You can open this file using Visual Studio IDE. This project was developed in Visual Studio 2015.

## Projects

### MyVipCity

This folder contains the files for the main project of the solution, MyVIPCity. This project is a .NET MVC Application that contains the definition of the MVC controllers, WebApi controllers, all javascript files developed for the project, as well as all the vendor files.

MVC controllers are located under the folder **/Controllers** and the WebApi controllers are located under **/Controllers/api**.
The Javascript files for this project are located under **/Scripts/modules** and the vendor files under **/Scripts/vendors**. All the javascript code written for the project is written using the **Asynchronous module definition** and we used RequireJS as our file and module loader.

### MyVipCity.BusinessLogic

This project is a Class Library that contains the contracts (interfaces) and actual implementations of classes that handle the business logic.

### MyVipCity.Common

The purpose of this Class Library is to add code that can be shared across all projects of the application. It currently has only one interface **IResolver** and a corresponding specific implementation **SimpleInjectorResolver**.

### MyVipCity.CompositionRoot

This is the project where the IoC container used in this project, [SimpleInjector](https://simpleinjector.org/index.html), is configured

### MyVipCity.DataTransferObjects

In this class library, we define the Data Transfer Objects (DTOs) used in the application. There is a DTO for each domain (see **MyVipCity.Domain**) class defined. i.e BusinessDto <-> Business. DTOs are used in the WebApi REST endpoints to receive/sent data from/to the client. DTOs are also sent to the BusinessLogic layer for further processing.

### MyVipCity.Domain

In this project, we define all domain classes. ie. Business, PromoterProfile, AttendingRequest, Picture, ProfilePicture, Review, etc. Most of this classes have a corresponding table in the database.

### MyVipCity.Domain.Automapper

There is a need to convert DTOs object into domain objects before running business rules and before persisting these objects into the database. There is also a need to convert domain objects into DTOs before sending this objects back to the client. Here is where we use [Automapper](http://automapper.org/) to easily execute this conversions between objects. 

In this project we define the configuration for Automapper; that is, we define how one specific object maps into another.

### MyVipCity.Domain.Mappings.EF

We use Entity Framework as out Object-relational mapping (ORM) tool. Here we define how the domain classes map into the corresponding tables in the database. We also define the relationships between this tables. i.e One-To-Many, Many-To-Many.

### MyVipCity.IpGeolocation

In this project, we define some interfaces and classes that allow us to approximate where a given IP address is located in the map; that is, to find the latitude and longitude of a given IP address. 

The key interface here is **IpGeolocator**. At this moment, we use [IPInfoDB API](http://ipinfodb.com/ip_location_api.php) as a service to locate the user given the IP address, so we defined the class **IpInfoDbGeoLocator**. If you want to use another service to accomplish this task, you just need to create a new class that implements **IpGeolocator** and then in the CompositionRoot project change the configuration of the IoC container to user your new class.

### MyVipCity.Mailing.Contracts

Here we define an interface for each transactional email that we need to send as well as the corresponding EmailModel.

### MyVipCity.Mailing.Sendgrid

In this class library we provide a concrete implementation for each interface defined in **MyVipCity.Mailing.Contracts**. Emails are sent using [SendGrid API](https://sendgrid.com/), so we have the class **SendGridEmailService** that implements all those interfaces.

If you need or want to use another service to sent the emails in the application, you just need to create a class that implements all the interfaces in **MyVipCity.Mailing.Contracts** and modify the configuration of the IoC container in the CompositionRoot project.

## Secrets

There is an important file that must exists at the same level of the folder **MyVipCity**. This file must be named **AppSettingsSecrets.config** and it contains all the secrets used in the application, like API keys. See [Best practices for deploying passwords and other sensitive data to ASP.NET and Azure App Service](https://docs.microsoft.com/en-us/aspnet/identity/overview/features-api/best-practices-for-deploying-passwords-and-other-sensitive-data-to-aspnet-and-azure)

```
<appSettings>  
	<!-- Google OAuth: External login -->
	<add key="myvipcity:google-client-id" value="<value here>" />	
	<add key="myvipcity:google-client-secret" value="<value here>" />
	
	<!-- Google Geocoding Api: Converts address into geographic coordinates -->
	<add key="myvipcity:google-geocoding-api-key" value="<value here>" />
	
	<!-- Google Javascript Maps Api: Show maps and add markers -->
	<add key="myvipcity:google-maps-api-key" value="<value here>" />
	
	<!-- Facebook OAuth: External login -->
	<add key="myvipcity:fb-app-id" value="<value here>" />
	<add key="myvipcity:fb-app-secret" value="<value here>" />
	
	<!-- Send Grid: email service -->
	<add key="myvipcity:send-grid-api" value="<value here>" />
	
	<!-- IP Info Db: find geographic coordinates from an IP address -->
	<add key="myvipcity:ip-info-db-api-key" value="<value here>" />
</appSettings>  
```
**Important**: never upload this file into a public repository, after all what you have here is a secret. SendGrid, for example, has a GitHub crawler that search for keys they have issued, and if they find one, they will lock your key and you won't be able to send emails anymore until you: 1) Remove the key from the repo 2) Change the key 3) Communicate with them (*it happened to me, and trust me, you don't want to go through this.*)

You can easily obtain your own key.

[Google] (https://console.developers.google.com)

[Facebook](https://developers.facebook.com/)

[IP Info DB](http://ipinfodb.com/ip_location_api.php)

## Getting the project up and running

### Secrets file

Make sure you have the secrets file as described above.

### Deploying the Database

We used [Entity Framework Code First](http://www.entityframeworktutorial.net/code-first/what-is-code-first.aspx), so that means that we generate our database from our Domain Classes. There are no .sql files in this project.

1) Open Visual Studio
2) Open the Web.config file under the project **MyVipCity** and change the connection string to point to your server and desired database name.
3) Open the Package Manager Console, select MyVipCity as the default project
4) Type **update-database** and hit ENTER. 

After that, the database will be created

Click RUN from inside Visual Studio and that's it. We recommend installing IIS in your development machine and adding an application for MyVipCity, that way you don't need to hit the RUN button every time you make a change.




