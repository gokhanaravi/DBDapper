# DBDapper
[![NuGet](http://img.shields.io/nuget/vpre/DBDapper.svg?label=NuGet)](https://www.nuget.org/packages/DBDapper/)
## Overview

`DBDapper` is a utility class for interacting with a SQL Server database using Dapper. It provides convenient methods for executing stored procedures and SQL queries and handling database connections.

## Table of Contents

- [Getting Started](#getting-started)
  - [Installation](#installation)
    - [Using .NET CLI](#using-net-cli)
    - [Using Package Manager Console](#using-package-manager-console)
  - [Configuration](#configuration)
- [Usage](#usage)
  - [Initializing DBDapper](#initializing-dbdapper)
  - [Executing Stored Procedures](#executing-stored-procedures)
  - [Executing SQL Queries](#executing-sql-queries)
- [Contributing](#contributing)
- [License](#license)

## Getting Started

### Installation

#### Using .NET CLI

To install the required packages using .NET CLI, run the following command in the terminal:

```bash
dotnet add package DBDapper
```

#### Using Package Manager Console

To install the required packages using Package Manager Console, run the following command in the console:

```powershell
Install-Package DBDapper
```

### Configuration

Make sure to configure your database connection string in the `appsettings.json` file:

```json
{
  "ConnectionStrings": {
    "YourConnectionStringName": "YourActualConnectionStringHere"
  }
}
```

Replace `"YourConnectionStringName"` with the desired name and `"YourActualConnectionStringHere"` with your actual database connection string.

## Usage

### Initializing DBDapper

```csharp
// Create an instance of DBDapper
DBDapper dbDapper = new DBDapper();

// OR specify a connection string from appsettings.json
DBDapper dbDapper = new DBDapper("YourConnectionStringName");
```

### Executing Stored Procedures

#### Synchronous

```csharp
// Execute a stored procedure and get the results
List<YourObjectType> results = dbDapper.RunSqlProc<YourObjectType>("YourStoredProcedure", parameters);
```

#### Asynchronous

```csharp
// Execute a stored procedure asynchronously and get the results
List<YourObjectType> results = await dbDapper.RunSqlProcAsync<YourObjectType>("YourStoredProcedure", parameters);
```

### Executing SQL Queries

#### Synchronous

```csharp
// Execute a SQL query and get the results
List<YourObjectType> results = dbDapper.RunSqlQuery<YourObjectType>("YourSqlQuery", parameters);
```

#### Asynchronous

```csharp
// Execute a SQL query asynchronously and get the results
List<YourObjectType> results = await dbDapper.RunSqlQueryAsync<YourObjectType>("YourSqlQuery", parameters);
```

## Contributing

If you'd like to contribute to `DBDapper`, please follow these steps:

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Make your changes and ensure tests pass.
4. Submit a pull request.

## License

This project is licensed under the terms of the [LICENSE.txt](LICENSE.txt) file.
