using CityCongestionCharge.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CityCongestionCharge.DesktopUI;

/// <summary>
/// View model for the main window.
/// </summary>
/// <remarks>
/// Note that the class does not need to implement <see cref="INotifyPropertyChanged"/> because
/// properties are not changed in C#. Instead, the properties are only changed in the XAML file 
/// using bindings.
/// </remarks>
public class MainWindowViewModel
{
    private readonly CccDataContext context;

    /// <summary>
    /// Helper class to display car type descriptions in a combo box.
    /// </summary>
    public class CarTypeDescription
    {
        public CarType? CarType { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public List<CarTypeDescription> CarTypes { get; } =
    [
        new() { CarType = null, Description = "All" },
        new() { CarType = CarType.PassengerCar, Description = "Passenger Car" },
        new() { CarType = CarType.Van, Description = "Van" },
        new() { CarType = CarType.Lorry, Description = "Lorry" },
        new() { CarType = CarType.Motorcycle, Description = "Motorcycle" },
    ];

    public ObservableCollection<Detection> Detections { get; set; } = [];
    public CarTypeDescription SelectedCarType { get; set; }
    public bool OnlyInside { get; set; } = false;
    public bool OnlyMultiCarDetections { get; set; } = false;
    public string LicensePlateFilter { get; set; } = string.Empty;

    public DelegateCommand RefreshCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainWindowViewModel(CccDataContext context)
    {
        this.context = context;
        SelectedCarType = CarTypes[0];
        RefreshCommand = new DelegateCommand(async (_) => await Refresh());
    }

    public async Task Refresh()
    {
        throw new NotImplementedException();
    }
}
