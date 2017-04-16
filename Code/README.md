
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
