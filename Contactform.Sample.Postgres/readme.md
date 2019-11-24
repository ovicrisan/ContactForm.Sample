# ContactForm.Sample.Postgres

This is an web API project used to connect to a Postgres instance, either installed or from a Docker container, create the database and apply migration, and finally, save the data. 
Is an infrastructure project that isolate all work with the database. In future versions I intend to add more database providers, for alternative persistence, similar to this one, all of them with its own API endpoints.

The endpoint of this deployed project is specified in settings or environment variables of main ContactForm.Sample project.

## Data model

I defined a *ContactModelData* class inherited from *ContactForm.ContactModel* class which just adds a GUID Id and a CreationDate field, so we know when a new ContactForm record was created.

## References

First step was to add a reference to Entity Framework Core (including design tools needed for migrations) and Postgres drivers, notice these in *.csproj file:

```
<PackageReference Include="Npgsql" Version="4.1.1" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="3.0.1" />
```

## Settings

The only settings this project needs is the connection string to the Postgres database. 
You can change it in *appsettings.json* when run locally, or via *ConnectionStrings__ContactFormSampleDB* environment variable for Docker, like this example from *docker-compose.yml*:

```
environment:
      - ConnectionStrings__ContactFormSampleDB=Host=postgres;Database=contactform;Username=postgres;Password=postgres
```

Thsi API is listening on post 8000, set up in *Program.cs*.

AppDbContext was initialized in Starup.cs using dependency injection.

## Migrations

After defining *ContactModelData.cs* and *AppDbContext.cs* I used EF Core migrations to generate code to create tables and index.
Also *Microsoft.EntityFrameworkCore.Design* package was added as a reference.

Then I used Package Manager console in Visual Studio, choosing *Contactform.Sample.Postgres* as default project:

```
Add-Migration Init
Update-Database
```

## Apply migrations in code

Generated migrations files are also applied at runtime for Docker deployments, using *Extensions/MigrationManager.cs* class.
Notice in *Program.cs* call of *MigrateDatabase()* static function:

```
CreateHostBuilder(args)
    .Build()
    .MigrateDatabase()
    .Run();
```

If the Postgres instance is running properly and connection string is setup correctly this call will apply migration files and create database, tables and indexes.
There is no need to seed any data.

## Access Postgres DB from Docker compose instance

When deploying the projects with Docker compose, all from Docker comntainers, the easiest way to access Postgres instance is via *psql* command line:

Get remote shell into the *postgres* container with `docker exec -it postgres /bin/bash`, then

```
psql -U postgres -W
\l
\c contactform
select * from "Contacts";
\q
```
