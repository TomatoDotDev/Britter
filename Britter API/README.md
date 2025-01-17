# Britter API

## About
This repository contains the backend for the Britter Discussion Forum project completed as part of 6COM1070 Software Engineering Practice module.

The solution comprises of a .NET web API that interfaces to a SQLite database for persistent storage.

Projects are arranged as so:

1. Britter.API - .NET web API using MVC approach
2. Britter.DataAccess - Data and Access layer utilising Entity Framework Core, along with internal db models.
4. Britter.DTOs - Data Transfer Object definitions, those that are consumed and returned by the web API.
5. *.UnitTests - Unit test projects using xunit.

## Authors
Tom Lawrence - tl21aar@herts.ac.uk
## Software Engineering features
- Unit testing of all software items
- Methodical and modular layout of solution
- Well-documented codebase
- Github actions for pipeline running on each commit.
- Use of SOLID, DRY and KISS development principles

## Running the solution
Ensuring you are running the project on Visual Studio 2022, to run the solution, complete the following:
1. Select `Build` from the navigation menu and select `Clean solution`.
1. Select `Build` from the navigation menu and select `Build solution` or `Ctrl+Shift+B`.
1. Press `F5` to start debugging.
1. To view the API development client, navigate to either `https://localhost:7013/scalar/v1` or `http://localhost:5297/scalar/v1`.

## Viewing the API definition
Providing you have built and run the solution as above, upon navigating to the aforementioned URIs, should be presented with a UI frontend courtesy of Scalar.

From this UI, a client library can be selected from the list and then for each endpoint, client-side code on how to call the endpoint will be provided.

## Future improvements
- Extending the login and registration system to be able to use Microsoft accounts (those used at UoH).
- Separating the solution into microservices e.g. user management, post management, report management etc.
- Shifting the database provider to PostgreSQL for better performance and scalability.
- Implementing a caching layer to reduce the number of database calls.
- Implementing a message broker for asynchronous processing of tasks.
- Containerising the solution using Docker for easier deployment and scaling.