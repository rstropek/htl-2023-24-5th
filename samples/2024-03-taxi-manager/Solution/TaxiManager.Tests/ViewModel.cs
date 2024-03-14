using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.ComponentModel;
using TaxiManager.Data;

namespace TaxiManager.Tests
{
    public class ViewModelTests
    {
        [Fact]
        public void ImplementINotifyPropertyChanged()
        {
            var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
            var vm = new MainWindowViewModel(dbContext);
            Assert.IsAssignableFrom<INotifyPropertyChanged>(vm);

            var notifiedPropertyNames = new List<string>();
            vm.PropertyChanged += (_, ea) => notifiedPropertyNames.Add(ea.PropertyName!);
            vm.Charge = Decimal.MaxValue;
            vm.SelectedDriver = new Driver();
            vm.SelectedTaxi = new Taxi();

            Assert.Equal(["Charge", "SelectedDriver", "SelectedTaxi"], notifiedPropertyNames);
        }

        [Fact]
        public void ImplementINotifyCollectionChanged()
        {
            var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
            var vm = new MainWindowViewModel(dbContext);
            VerifyNotifyCollectionChanged(vm.Taxis, () => vm.Taxis.Add(null!));
            VerifyNotifyCollectionChanged(vm.Drivers, () => vm.Drivers.Add(null!));
            VerifyNotifyCollectionChanged(vm.CompletedRides, () => vm.CompletedRides.Add(null!));
            VerifyNotifyCollectionChanged(vm.OngoingRides, () => vm.OngoingRides.Add(null!));
        }

        [Fact]
        public async Task Initialize()
        {
            var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
            await dbContext.ClearAsync();
            await dbContext.AddDummyRideAsync();
            var (_, _, rideID) = await dbContext.AddDummyRideAsync();
            await dbContext.EndRideAsync(rideID, 99m);

            var vm = new MainWindowViewModel(dbContext);
            await vm.InitAsync();

            Assert.Equal(2, vm.Taxis.Count);
            Assert.Equal(2, vm.Drivers.Count);
            Assert.Single(vm.OngoingRides);
            Assert.Single(vm.CompletedRides);
        }

        [Fact]
        public async Task EndRide()
        {
            var dbContext = new ApplicationDbContextFactory().CreateDbContext([]);
            await dbContext.ClearAsync();
            await dbContext.AddDummyRideAsync();

            var hasNotified = false;
            var vm = new MainWindowViewModel(dbContext);
            vm.PropertyChanged += (_, ea) => hasNotified = true;

            await vm.InitAsync();
            vm.SelectedOngoingRide = vm.OngoingRides.First();
            vm.Charge = 99m;
            await vm.EndRideAsync();

            var ongoingRides = await dbContext.Rides.CountAsync(r => !r.Charge.HasValue || !r.End.HasValue);
            Assert.Equal(0, ongoingRides);
            Assert.True(hasNotified);
        }

        private static void VerifyNotifyCollectionChanged(object target, Action changeAction)
        {
            var notifyingTarget = target as INotifyCollectionChanged;
            Assert.NotNull(notifyingTarget);

            var hasNotified = false;
            notifyingTarget.CollectionChanged += (_, __) => hasNotified = true;
            changeAction();
            Assert.True(hasNotified);
        }
    }
}