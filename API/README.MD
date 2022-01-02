# Backend REST API

# Setting up development environment

To run the backend after cloning the repository the following steps are necessary:
* Install the [.NET Core SDK](https://www.microsoft.com/net/download/windows) 
* **dotnet restore** to download the necessary packages.
* **dotnet build** to generate the dlls.
* **dotnet run** to start the application.
* **dotnet test** To run the tests.
* You can also just open the project in Visual Studio and let the IDE do everything for you.

dotnet tool install --global dotnet-ef --version 3.1.0

Creating migrations
* **dotnet ef migrations add {MigrationName} --project Backend.DAL --startup-project Backend.Api** to scaffold a migration and create models defined in DatabaseContext.

Updating the database based on it
* **dotnet ef database update --project Backend.DAL --startup-project Backend.Api** to apply the new migration to the database. This command creates the database before applying migrations.

An easy way to see the exposed endpoints that clients can consume is to start the application and in your favorite browser navigate to https://localhost:5001/swagger/index.html.

# Architecture
 * Backend.Api 
 * Backend.Api.Core
 * Backend.Api.DAL

# Docs and useful links
## Asynchronous code in C#
https://www.c-sharpcorner.com/article/task-and-thread-in-c-sharp/