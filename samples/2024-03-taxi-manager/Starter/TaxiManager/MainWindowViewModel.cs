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
            // Use dependency injection to get the database context
            // Use delegate commands StartRideCommand and EndRideCommand to handle the start and end ride buttons
            //   (ride can only be started if a driver and a taxi are selected,
            //   ride can only be ended if an ongoing ride is selected and the charge is not empty)
            throw new NotImplementedException();
        }

        public async Task InitAsync()
        {
            // Fill the Taxis and Drivers collections with data from the database.
            // Note that this method will be called from MainWindow.xaml.cs when the window has been loaded.

            await RefreshRidesAsync();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        #region Properties
        // Note that the following properties are NOT YET data-binding friendly.
        // Change them accordingly!

        public List<Taxi> Taxis { get; } = [];

        public List<Driver> Drivers { get; } = [];

        public List<TaxiRide> OngoingRides { get; } = [];

        public List<TaxiRide> CompletedRides { get; } = [];

        public Driver? SelectedDriver { get; set; }

        public Taxi? SelectedTaxi { get; set; }

        public decimal? Charge { get; set; }

        public TaxiRide? SelectedOngoingRide { get; set; }
        #endregion

        public DelegateCommand StartRideCommand { get; }

        public DelegateCommand EndRideCommand { get; }

        public async Task StartRideAsync()
        {
            // Start a new ride with the selected taxi and driver (StartRideAsync method in the database context).
            // Refresh the rides after the ride has been started (RefreshRidesAsync method).
            throw new NotImplementedException();
        }

        public async Task EndRideAsync()
        {
            // End the selected ongoing ride with the given charge (EndRideAsync method in the database context).
            // Refresh the rides after the ride has been ended (RefreshRidesAsync method).
            throw new NotImplementedException();
        }

        private async Task RefreshRidesAsync()
        {
            // Clear everything in CompletedRides and OngoingRides and refill them with data from the database
            throw new NotImplementedException();
        }
    }
}
