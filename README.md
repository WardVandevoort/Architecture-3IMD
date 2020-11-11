# Architecture-3IMD (Flowershop)

## Introduction 

This project contains a simple Api, which functions as a management tool for flowershops. Additionaly it also includes unit and integration testing.

## Installation

You will need the following software:

- [.NET Core (At least version 3.1)](https://dotnet.microsoft.com/download).

We also included a database export so you can easily create a database with dummy data. You simply need to:

- Create a database and name it "architecture".
- Import the database_export.sql file, which you can find in the root folder.
- Go to the Api folder and add your database connection settings on line 48 of the Startup.cs file.

## Usage

You can use `dotnet watch run` to run the project; after you get the notification that the application started navigate to <http://localhost:5000/swagger/index.html> or <https://localhost:5001/swagger/index.html>; you will get an overview with all the API methods and a quick method to execute them.

## TODO

Momenteel hebben we enkel de basis endpoints, namelijk:
- Get, Get{id} en Post voor boeketten.
- Get, Get{id} en Post voor winkels.
- Get, Get{id}, Post en Update voor de verkopen.
De andere routes die in de jaaropdracht beschreven staan hebben we nog niet kunnen implementeren.

Verder hebben we ook enkele unit en integratie testen toegevoegd. De unit testen werken zoals het hoort, maar somige van de integratie testen geven af en toe een fout en we weten niet wat het probleem is.



