# EF Core Checklist

## One-Time Setup

```bash
dotnet tool install --global dotnet-ef
```

```bash
dotnet tool update
```

## Add NuGet Packages

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

## Start SQL Server (Docker)

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=MyStr0ngPazzword" -e "MSSQL_PID=Express" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

## SQL Server (Windows)

Install [SQL Server Express LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16)

## Access Database Interactively

Install [Azure Data Studio](https://learn.microsoft.com/en-us/azure-data-studio/download-azure-data-studio).

## Database Context

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

```cs
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

## Connection String in `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TodoEf;User=sa;Password=MyStr0ngPazzword;TrustServerCertificate=true"
  },
  ...
}
```

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\MSSQLLocalDB;Database=TodoEf;Integrated Security=true"
  },
  ...
}
```

## _csproj_ Setting (Linux)

```xml
<InvariantGlobalization>false</InvariantGlobalization>
```

## Add Migration

```bash
dotnet ef migrations add ...
```

