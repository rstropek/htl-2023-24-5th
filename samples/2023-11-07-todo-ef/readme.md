# EF Core Checklist

## One-Time Setup

You need to install the Entity Framework Core tool globally.

```bash
dotnet tool install --global dotnet-ef
```

This is how you can update the tool.

```bash
dotnet tool update --global dotnet-ef
```

## Add NuGet Packages

For a .NET application that uses Entity Framework Core, you need to install two NuGet packages. Note that the second one depends on the RDBMS you want to use. This example uses SQL Server (de facto standard for final exam here in this school). If you would use e.g. SQLite, you would install `Microsoft.EntityFrameworkCore.Sqlite` instead.

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

## Start SQL Server (Docker)

If you want to run SQL Server locally on Linux, you can use Docker.

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=MyStr0ngPazzword" -e "MSSQL_PID=Express" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

## SQL Server (Windows)

If you want to run SQL Server locally on Windows, you could use Docker, too. However, you can also install the free SQL Server Express LocalDB.

Install [SQL Server Express LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16)

## Access Database Interactively

To interactively explore the content of a database, install [Azure Data Studio](https://learn.microsoft.com/en-us/azure-data-studio/download-azure-data-studio). It works on all operating systems.

## Database Context

Here is an example for a DB context:

```cs
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    ...
}
```

This is how you add a DB context to ASP.NET Core Dependency Injection.

```cs
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

## Connection String in `appsettings.json`

Here is an example for a connection string for a SQL Server running in Docker.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TodoEf;User=sa;Password=MyStr0ngPazzword;TrustServerCertificate=true;Encrypt=false"
  },
  ...
}
```

Here is one for SQL Server Express LocalDB.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\MSSQLLocalDB;Database=TodoEf;Integrated Security=true"
  },
  ...
}
```

## _csproj_ Setting (Linux)

Note that under Linux, you need to add the following setting to your _.csproj_ file.

```xml
<InvariantGlobalization>false</InvariantGlobalization>
```

## Add Migrations

```bash
dotnet ef migrations add ...
```

## Update Database Structure

```bash
dotnet ef database update
```
