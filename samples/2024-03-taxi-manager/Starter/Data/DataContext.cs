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

    /// <summary>
    /// Deletes all rows from all tables (taxis, drivers, rides)
    /// </summary>
    public async Task ClearAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds a taxi to the database
    /// </summary>
    /// <param name="newTaxi">Taxi data that should be added</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="newTaxi"/> is null</exception>
    /// <returns>ID of the newly added taxi as created by the database</returns>
    public async Task<int> AddTaxiAsync(Taxi newTaxi)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds a driver to the database
    /// </summary>
    /// <param name="newDriver">Driver data that should be added</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="newDriver"/> is null</exception>
    /// <returns>ID of the newly added driver as created by the database</returns>
    public async Task<int> AddDriverAsync(Driver newDriver)
    {
        throw new NotImplementedException();

    }

    /// <summary>
    /// Adds a new ongoing taxi ride to the database
    /// </summary>
    /// <param name="taxi">Taxi doing the ride</param>
    /// <param name="driver">Driver driving the taxi</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="taxi"/> or <paramref name="driver"/> are null</exception>
    /// <returns>ID of the newly added taxi ride as created by the database</returns>
    /// <remarks>
    /// The method sets <see cref="TaxiRide.Start"/> to the current system time (<see cref="DateTime.Now"/>).
    /// <see cref="TaxiRide.End"/> and <see cref="TaxiRide.Charge"/> are null. These values will be set
    /// when the taxi ride will be ended (see <see cref="EndRideAsync"/>.
    /// </remarks>
    public async Task<int> StartRideAsync(Taxi taxi, Driver driver)
    {
        throw new NotImplementedException();

    }

    /// <summary>
    /// Completes an existing ongoing taxi ride
    /// </summary>
    /// <param name="rideID">ID of the taxi ride that should be completed (previously returned by <see cref="StartRideAsync"/>)</param>
    /// <param name="charge">Charge of the taxi ride</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="charge"/> is negative</exception>
    /// <exception cref="ArgumentException">Thrown if no ride with ID <paramref name="rideID"/> exists</exception>
    /// <remarks>
    /// The method sets <see cref="TaxiRide.End"/> to the current system time (<see cref="DateTime.Now"/>)
    /// and stores <paramref name="charge"/> in <see cref="TaxiRide.Charge"/>.
    /// </remarks>
    public async Task EndRideAsync(int rideID, decimal charge)
    {
        throw new NotImplementedException();

    }

    /// <summary>
    /// Returns a statistic about the total charge per driver for a given month
    /// </summary>
    /// <param name="year">Year filter</param>
    /// <param name="month">Month filter</param>
    /// <returns>
    /// List of <see cref="DriverStatistics"/> objects containing the total charge for each driver in the given month.
    /// </returns>
    public async Task<List<DriverStatistics>> GetDriverStatisticsAsync(int year, int month)
    {
        throw new NotImplementedException();
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