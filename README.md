# TestContainersArticle

## Description

This project demonstrates how to use TestContainers and Respawn write tests that run against a real database.

## Installation
Clone the repository  

```bash
git clone https://github.com/henrychris/TestContainersArticle.git
```

In `TestContainersArticle.Main`, create an `appsettings.Development.json` file. Add the following section:

```json
"DatabaseSettings": {
    "ConnectionString": "Server=localhost;Database=TestContainersDb;Port=5432;User Id=postgres;Password=postgres;"
  }
```

## Usage

cd to the root directory  

```bash
cd testcontainersarticle
```

run tests

```bash
dotnet test
```

You can run the project using the `dotnet run` command in the `TestContainersArticle.Main` directory or by hitting ctrl + F5 while in VS 2022 / VS Code. The project will be available at `http://localhost:5051` or `https://localhost:7088`.

Access `http://localhost:5051/swagger` or `https://localhost:7088/swagger` to see the available endpoints.
