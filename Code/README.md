
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

There is a need to convert DTOs object into domain objects before running business rules and before persisting this objects into the database. There is also a need to convert domain objects into DTOs before sending this objects back to the client. Here is where we use [Automapper](http://automapper.org/) to easily execute this conversions between objects. 

In this project we define the configuration for Automapper; that is, we define how one specific object maps into another.


