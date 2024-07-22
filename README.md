Sales Analysis API
=================

A simple and efficient API for analyzing sales data, built with a focus on clean architecture, performance, and scalability. This project allows you to query your sales data in various ways, such as by date range, goods, or clients, and returns the results in a fast and reliable manner.

Table of Contents
-----------------

* [Features](#features)
* [Architecture](#architecture)
* [Technologies](#technologies)
* [Getting Started](#getting-started)
* [API Endpoints](#api-endpoints)
* [Contributing](#contributing)

Features
--------

* Query sales data by date range, goods, or clients
* Caching for improved performance and reduced database load
* Clean, modular architecture for easy maintenance and scalability
* Detailed documentation for easy integration and usage

Architecture
------------

The Sales Analysis API follows a clean architecture pattern, with separate layers for presentation, application, domain, and infrastructure. This approach ensures a clear separation of concerns, making the codebase easy to maintain and scale.

Technologies
------------

* ASP.NET Core: A cross-platform, high-performance framework for building web APIs.
* Entity Framework Core: A lightweight, extensible, open-source, and cross-platform data access technology.
* PostgreSQL: A powerful, open-source object-relational database system.
* Redis: An in-memory data structure store, used as a cache to improve performance.
* Dapper: A simple, fast, and lightweight object-relational mapper (ORM) for .NET.
* OAuth 2.0: Google authorization for safety purposes

Getting Started
---------------

1. Clone the repository: `git clone https://github.com/TheOriginalMJKey/Parser-Microservice.git`
2. Install the required packages: `dotnet restore`
3. Configure the database connection string in `appsettings.json`
4. Run the migrations: `dotnet ef database update`
5. Start the application: `dotnet run`

API Endpoints
-------------

* `GET /sales`: Get sales data by date range (optional filters: customer name, goods name)
* `GET /salesbygoods`: Get sales data by goods (filters: start date, end date, goods name)
* `GET /salesbycustomers`: Get sales data by clients (filters: start date, end date, client name)
* `GET /salesbymonth`: Get sales data by month (filter: year)
* `POST /orders/delete`: deletes existing order
* `POST /orders/create`: creates order

Contributing
------------

We welcome contributions from the community. If you wish to contribute, please fork the repository and submit a pull request with your proposed changes. Please make sure to follow the existing code style and include appropriate tests and documentation for any new features or changes.
