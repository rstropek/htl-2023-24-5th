using CityCongestionCharge.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace CityCongestionCharge.DesktopUI;

/// <summary>
/// View model for the database administration window.
/// </summary>
public class DbAdminWindowViewModel : INotifyPropertyChanged
{
    private readonly DemoDataWriter writer;

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbAdminWindowViewModel"/> class.
    /// </summary>
    /// <param name="writer">The demo data writer.</param>
    public DbAdminWindowViewModel(DemoDataWriter writer)
    {
        this.writer = writer;
    }

    /// <summary>
    /// Gets the list of confirmation options.
    /// </summary>
    public List<string> Confirmations { get; } = [string.Empty, "Yes, I confirm"];

}
