# Architecture-3IMD (Flowershop)

## Introduction 

This project contains a simple Api, which functions as a management tool for flowershops. Additionaly it also includes unit and integration testing.

## Installation

You will need the following software:

- [.NET Core (At least version 5)](https://dotnet.microsoft.com/download).
- LAMP stack of your choosing.

Before you can use the project you wil need to change the database connection string in the appsettings.Development.json file, this file can be found in the root of the Architecture_3IMD.Api folder. !!! You do not need to change the database name !!!

"ConnectionStrings": {
    "SQL": `"server=localhost;user=(your username);password=(your password);database=architecture"`
},

We included migrations and seeders so you can easily create a database with dummy data. You simply need to run the command `dotnet ef database update`.

## Usage

You can use `dotnet watch run` to run the project; after you get the notification that the application started navigate to <http://localhost:5000/swagger/index.html> or <https://localhost:5001/swagger/index.html>; you will get an overview with all the API methods and a quick method to execute them.

You can use `dotnet test` to run the tests, if the project is working correctly all these tests should be passed.

You can use `docker-compose build` to build the projects docker containers.

## Hoe ver zijn we geraakt?

We hebben alles dat in de jaaropdracht staat kunnen implementeren. We hebben niet zo veel unit/integratie testen, dit komt onder andere doordat de basis register API van Vlaanderen problemen gaf om de POST en UPDATE van de stores te testen. Verder zijn we er ook niet uit gekomen hoe we de sales controller moeten testen, aangezien deze anders werkt door het gebruik van MongoDB.



