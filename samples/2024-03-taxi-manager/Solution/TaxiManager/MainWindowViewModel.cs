using Microsoft.EntityFrameworkCore;
using ProductHierarchy;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TaxiManager.Data;

namespace TaxiManager
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel(TaxiDataContext dbContext)
        {
            this.dbContext = dbContext;
            StartRideCommand = new DelegateCommand(this, async (_) => await StartRideAsync(), (_) => SelectedDriver != null && SelectedTaxi != null);
            EndRideCommand = new DelegateCommand(this, async (_) => await EndRideAsync(), (_) => SelectedOngoingRide != null && Charge != null);
            PropertyChanged += (_, __) =>
            {
                StartRideCommand.RaiseCanExecuteChanged();
                EndRideCommand.RaiseCanExecuteChanged();
            };
        }

        public async Task InitAsync()
        {
            foreach(var taxi in await dbContext.Taxis.ToArrayAsync())
            {
                Taxis.Add(taxi);
            }
                
            foreach(var driver in await dbContext.Drivers.ToArrayAsync())
            {
                Drivers.Add(driver);
            }

            await RefreshRidesAsync();
        }

        public ObservableCollection<Taxi> Taxis { get; } = [];

        public ObservableCollection<Driver> Drivers { get; } = [];

        public ObservableCollection<TaxiRide> OngoingRides { get; } = [];

        public ObservableCollection<TaxiRide> CompletedRides { get; } = [];

        private Driver? selectedDriver;
        public Driver? SelectedDriver
        {
            get { return selectedDriver; }
            set
            { 
                selectedDriver = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDriver)));
            }
        }

        private Taxi? selectedTaxi;
        public Taxi? SelectedTaxi
        {
            get { return selectedTaxi; }
            set
            {
                selectedTaxi = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTaxi)));
            }
        }

        private decimal? charge;
        public decimal? Charge
        {
            get { return charge; }
            set
            {
                charge = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Charge)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private TaxiRide? selectedOngoingRide;
        public TaxiRide? SelectedOngoingRide
        {
            get { return selectedOngoingRide; }
            set
            {
                selectedOngoingRide = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedOngoingRide)));
            }
        }

        private readonly TaxiDataContext dbContext;

        public DelegateCommand StartRideCommand { get; }

        public DelegateCommand EndRideCommand { get; }

        public async Task StartRideAsync()
        {
            await dbContext.StartRideAsync(SelectedTaxi!, SelectedDriver!);
            await RefreshRidesAsync();
        }

        public async Task EndRideAsync()
        {
            await dbContext.EndRideAsync(SelectedOngoingRide!.ID, Charge ?? 0m);
            await RefreshRidesAsync();
        }

        private async Task RefreshRidesAsync()
        {
            CompletedRides.Clear();
            foreach (var r in await dbContext.Rides.Where(r => r.End != null).ToArrayAsync())
            {
                CompletedRides.Add(r);
            }

            OngoingRides.Clear();
            foreach (var r in await dbContext.Rides.Where(r => r.End == null).ToArrayAsync())
            {
                OngoingRides.Add(r);
            }
        }
    }
}
