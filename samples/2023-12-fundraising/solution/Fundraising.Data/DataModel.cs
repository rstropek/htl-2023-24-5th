using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fundraising.Data;

public class Campaign
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public List<Visit> Visits { get; set; } = [];
}

public class Household
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string TownName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string StreetName { get; set; } = string.Empty;

    [MaxLength(10)]
    public string HouseNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string FamilyName { get; set; } = string.Empty;

    public List<Visit> Visits { get; set; } = [];
}

public class Visit
{
    public int Id { get; set; }

    public int HouseholdId { get; set; }

    public Household? Household { get; set; }

    public int CampaignId { get; set; }

    public Campaign? Campaign { get; set; }

    public bool SuccessfullyVisited { get; set; }
}

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Campaign> Campaigns => Set<Campaign>();

    public DbSet<Household> Households => Set<Household>();

    public DbSet<Visit> Visits => Set<Visit>();

    public async Task<Campaign> CreateCampaign(string name)
    {
        var campaign = new Campaign { Name = name };
        await Campaigns.AddAsync(campaign);
        await SaveChangesAsync();
        return campaign;
    }
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(config["ConnectionStrings:DefaultConnection"]);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}