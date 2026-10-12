# Student Manager

Console application to manage students and staff, built with C# and .NET.

## Features

Users can take one of three roles: Director, Professor or Student.

- **Director:** opens or closes the semester and registers new professors from the list of users.
- **Professor:** records grades for students and searches their history.
- **Student:** enrolls in subjects and consults their grade history.
- All operations are stored in a PostgreSQL database.

## Tech stack

- C#
- .NET 9.0
- Npgsql 10.0.3
- PostgreSQL

## Getting started

### Requirements

- .NET SDK 9.0


### Run

```bash
git clone https://github.com/ArturoAras/<repository-name>.git
cd <StudentManager>
dotnet run
```

## Status

In progress. Next steps:

- Add a `Subject` class that stores the duration and the minimum number of grades per student.
- Add semester validation: when the semester is closed, no grades, enrollments or other operations can be saved.

## Author

**Arturo Ariel Aras** - [LinkedIn](https://www.linkedin.com/in/arturo-aras)
