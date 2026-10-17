# Student Manager

Console application to manage students and grades, built with C# and .NET.

## Features

Implemented:

- User registration and login.
- **Professor:** records grades per student and subject.
- **Student:** consults their grades.
- All data is stored in a PostgreSQL database.

Planned: Director role (open and close the semester, register professors), subject enrollment and semester validation.

## Tech stack

- C#
- .NET 9.0
- Npgsql 10.0.3
- PostgreSQL 18

## Getting started

### Requirements

- Git
- .NET SDK 9.0
- PostgreSQL 18

### Database setup

Create the database and load the schema:

```powershell
psql -U postgres -c "CREATE DATABASE campus_academic_db;"
psql -U postgres -d campus_academic_db -f database/schema.sql
```

The connection uses `localhost`, port `5432` and the `postgres` user. The password is read from the `STUDENTMANAGER_DB_PASSWORD` environment variable:

```powershell
setx STUDENTMANAGER_DB_PASSWORD "your_password"
```

Open a new terminal after running `setx`, so the variable is available.

### Run

```powershell
git clone https://github.com/ArturoAras/StudentManager.git
cd StudentManager
dotnet run --project StudentManager
```

## Status

In progress. Next steps:

- Add a `Subject` class that stores the duration and the minimum number of grades per student.
- Add semester validation: when the semester is closed, no grades, enrollments or other operations can be saved.

## Author

**Arturo Ariel Aras** - [LinkedIn](https://www.linkedin.com/in/arturo-aras)