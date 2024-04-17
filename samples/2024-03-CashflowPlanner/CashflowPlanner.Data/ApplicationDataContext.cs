using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CashflowPlanner.Data;

public class ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();
    public DbSet<Cashflow> Cashflows => Set<Cashflow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>().HasData(
            new Currency { ID = 1, Name = "USD" },
            new Currency { ID = 2, Name = "EUR" },
            new Currency { ID = 3, Name = "GBP" },
            new Currency { ID = 4, Name = "JPY" },
            new Currency { ID = 5, Name = "CNY" });

        modelBuilder.Entity<Category>().HasData(
            new Category { ID = 1, Name = "Product Sales", InOut = 'I' },
            new Category { ID = 2, Name = "Service Revenue", InOut = 'I' },
            new Category { ID = 3, Name = "Interest Income", InOut = 'I' },
            new Category { ID = 4, Name = "Dividend Income", InOut = 'I' },
            new Category { ID = 5, Name = "Rental Income", InOut = 'I' },
            new Category { ID = 6, Name = "Salaries and Wages", InOut = 'O' },
            new Category { ID = 7, Name = "Rent Expense", InOut = 'O' },
            new Category { ID = 8, Name = "Utilities Expense", InOut = 'O' },
            new Category { ID = 9, Name = "Insurance Expense", InOut = 'O' },
            new Category { ID = 10, Name = "Marketing and Advertising", InOut = 'O' },
            new Category { ID = 11, Name = "Research and Development", InOut = 'O' },
            new Category { ID = 12, Name = "Travel Expense", InOut = 'O' },
            new Category { ID = 13, Name = "Office Supplies", InOut = 'O' },
            new Category { ID = 14, Name = "Legal and Professional Fees", InOut = 'O' },
            new Category { ID = 15, Name = "Depreciation and Amortization", InOut = 'O' },
            new Category { ID = 16, Name = "Interest Expense", InOut = 'O' },
            new Category { ID = 17, Name = "Taxes", InOut = 'O' },
            new Category { ID = 18, Name = "Repairs and Maintenance", InOut = 'O' },
            new Category { ID = 19, Name = "Cost of Goods Sold", InOut = 'O' },
            new Category { ID = 20, Name = "Loan Payments", InOut = 'O' });

    }
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDataContext>
{
    public ApplicationDataContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDataContext>();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

        return new ApplicationDataContext(optionsBuilder.Options);
    }
}
