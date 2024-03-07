using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TaxiManager.Data;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace TaxiManager.Tests;

public class DataContextMethods
{
    [Fact]
    [Description("Verifies that taxis are created correctly")]
    public async Task AddTaxi()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        var (newTaxi, _) = await dbContext.AddDummyTaxiAsync();

        Assert.NotNull(await dbContext.Taxis.FirstAsync(t => t.LicensePlate == newTaxi.LicensePlate));
    }

    [Fact]
    [Description("Verifies that an exception is thrown if argument is null")]
    public async Task AddTaxiArgumentNull()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        await Assert.ThrowsAsync<ArgumentNullException>(() => dbContext.AddTaxiAsync(null));
    }

    [Fact]
    [Description("Verifies that the ID of the created record is returned correctly")]
    public async Task AddTaxiReturnsValidID()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        var newTaxi = await dbContext.AddDummyTaxiAsync();
        var insertedTaxi = await dbContext.Taxis.FirstAsync(t => t.ID == newTaxi.ID);

        Assert.NotNull(insertedTaxi);
        Assert.Equal(newTaxi.Taxi.LicensePlate, insertedTaxi.LicensePlate);
    }

    [Fact]
    [Description("Verifies that drivers are created correctly")]
    public async Task AddDriver()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        var newDriver = await dbContext.AddDummyDriverAsync();
        Assert.NotNull(await dbContext.Drivers.FirstAsync(d => d.Name == newDriver.Driver.Name));
    }

    [Fact]
    [Description("Verifies that an exception is thrown if argument is null")]
    public async Task AddDriverArgumentNull()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        await Assert.ThrowsAsync<ArgumentNullException>(() => dbContext.AddDriverAsync(null));
    }

    [Fact]
    [Description("Verifies that the ID of the created record is returned correctly")]
    public async Task AddDriverReturnsValidID()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        var newDriver = await dbContext.AddDummyDriverAsync();
        var insertedDriver = await dbContext.Drivers.FirstAsync(d => d.ID == newDriver.ID);

        Assert.NotNull(insertedDriver);
        Assert.Equal(newDriver.Driver.Name, insertedDriver.Name);
    }

    [Fact]
    [Description("Verifies that taxi rides can be started correctly")]
    public async Task StartRide()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        var (taxi, driver, _) = await dbContext.AddDummyRideAsync();
        var newRide = await dbContext.Rides.FirstAsync(r => r.Driver!.Name == driver.Name && r.Taxi!.LicensePlate == taxi.LicensePlate);

        Assert.NotNull(newRide);
        Assert.Equal(taxi, newRide.Taxi);
        Assert.Equal(driver, newRide.Driver);
        Assert.True(newRide.Start <= DateTime.Now && newRide.Start >= DateTime.Now.AddSeconds(-5));
    }

    [Fact]
    [Description("Verifies that an exception is thrown if argument is null")]
    public async Task StartRideArgumentNull()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        await Assert.ThrowsAsync<ArgumentNullException>(() => dbContext.StartRideAsync(null, null));
    }

    [Fact]
    [Description("Verifies that the ID of the created record is returned correctly")]
    public async Task StartRideReturnsValidID()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        var ride = await dbContext.AddDummyRideAsync();
        var newRide = await dbContext.Rides.FirstAsync(r => r.ID == ride.ID);

        Assert.NotNull(newRide);
        Assert.Equal(ride.Taxi, newRide.Taxi);
        Assert.Equal(ride.Driver, newRide.Driver);
    }

    [Fact]
    [Description("Verifies that taxi rides can be ended correctly")]
    public async Task EndRide()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        const decimal charge = 99m;
        var (taxi, driver, _) = await dbContext.AddDummyRideAsync();
        var newRide = await dbContext.Rides.FirstAsync(r => r.Driver!.Name == driver.Name && r.Taxi!.LicensePlate == taxi.LicensePlate);

        await dbContext.EndRideAsync(newRide.ID, charge);

        newRide = await dbContext.Rides.FirstAsync(r => r.ID == newRide.ID);
        Assert.True(newRide.End <= DateTime.Now && newRide.End >= DateTime.Now.AddSeconds(-5));
        Assert.Equal(charge, newRide.Charge);
    }

    [Fact]
    [Description("Verifies that an exception is thrown if argument is invalid")]
    public async Task EndRideNegativeCharge()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => dbContext.EndRideAsync(Int32.MaxValue, -1m));
    }

    [Fact]
    [Description("Verifies that an exception is thrown if invalid ride ID is provided")]
    public async Task EndRideInvalidID()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        await Assert.ThrowsAsync<ArgumentException>(() => dbContext.EndRideAsync(Int32.MaxValue, 1m));
    }

    [Fact]
    [Description("Verifies that clear deletes all data from the database")]
    public async Task Clear()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        await dbContext.AddDummyRideAsync();
        await dbContext.ClearAsync();

        Assert.Equal(0, await dbContext.Taxis.CountAsync());
        Assert.Equal(0, await dbContext.Drivers.CountAsync());
        Assert.Equal(0, await dbContext.Rides.CountAsync());
    }

    [Fact]
    [Description("Verifies that the driver statistic is calculated correctly (2 Points)")]
    public async Task GetDriverStatistics()
    {
        var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
        const decimal charge = 99m;
        await dbContext.ClearAsync();
        var (taxi, _) = await dbContext.AddDummyTaxiAsync();
        var (driver, _) = await dbContext.AddDummyDriverAsync();
        for (var i = 0; i < 5; i++)
        {
            var newRideID = await dbContext.StartRideAsync(taxi, driver);
            await dbContext.EndRideAsync(newRideID, charge);
        }

        var expected = new[] { new DriverStatistics { DriverName = driver.Name, TotalCharge = charge * 5 } };
        var actual = await dbContext.GetDriverStatisticsAsync(DateTime.Today.Year, DateTime.Today.Month);
        
        Assert.Equal(expected.Length, actual.Count);
        Assert.Equal(expected[0].DriverName, actual[0].DriverName);
        Assert.Equal(expected[0].TotalCharge, actual[0].TotalCharge);
    }
}
