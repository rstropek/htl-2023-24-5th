using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaxiManager.Data;

public class TaxiDataContext(DbContextOptions<TaxiDataContext> options) : DbContext(options)
{
    public DbSet<Taxi> Taxis => Set<Taxi>();

    public DbSet<Driver> Drivers => Set<Driver>();

    public DbSet<TaxiRide> Rides => Set<TaxiRide>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaxiRide>()
            .Property(r => r.Charge)
            .HasColumnType("decimal(18, 2)");
    }

    public async Task ClearAsync()
    {
        await Taxis.ExecuteDeleteAsync();
        await Drivers.ExecuteDeleteAsync();
        await Rides.ExecuteDeleteAsync();
    }

    public async Task<int> AddTaxiAsync(Taxi newTaxi)
    {
        ArgumentNullException.ThrowIfNull(newTaxi);

        Taxis.Add(newTaxi);
        await SaveChangesAsync();
        return newTaxi.ID;
    }

    public async Task<int> AddDriverAsync(Driver newDriver)
    {
        ArgumentNullException.ThrowIfNull(newDriver);

        Drivers.Add(newDriver);
        await SaveChangesAsync();
        return newDriver.ID;
    }

    public async Task<int> StartRideAsync(Taxi taxi, Driver driver)
    {
        ArgumentNullException.ThrowIfNull(taxi);
        ArgumentNullException.ThrowIfNull(driver);

        var newRide = new TaxiRide
        {
            Start = DateTime.Now,
            Taxi = taxi,
            Driver = driver
        };
        Rides.Add(newRide);
        await SaveChangesAsync();
        return newRide.ID;
    }

    public async Task EndRideAsync(int rideID, decimal charge)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(charge, 0m);

        if (!await Rides.AnyAsync(r => r.ID == rideID))
        {
            throw new ArgumentException(null, nameof(rideID));
        }

        var ride = await Rides.FirstAsync(r => r.ID == rideID);
        ride.End = DateTime.Now;
        ride.Charge = charge;
        await SaveChangesAsync();
    }

    public async Task<List<DriverStatistics>> GetDriverStatisticsAsync(int year, int month)
    {
        return await Rides.Where(r => r.Start.Year == year && r.Start.Month == month && r.Charge.HasValue)
            .GroupBy(r => r.Driver!.Name)
            .Select(r => new DriverStatistics
            {
                DriverName = r.Key,
                TotalCharge = r.Sum(x => x.Charge!.Value)
            })
            .ToListAsync();
    }
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<TaxiDataContext>
{
    public TaxiDataContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<TaxiDataContext>();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

        return new TaxiDataContext(optionsBuilder.Options);
    }
}