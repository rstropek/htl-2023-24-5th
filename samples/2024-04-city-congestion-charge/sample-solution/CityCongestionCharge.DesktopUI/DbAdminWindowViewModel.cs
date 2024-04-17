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
        ClearCommand = new DelegateCommand(this, (_) => OnClear(), (_) => DbActionsEnabled);
        FillCommand = new DelegateCommand(this, (_) => OnFill(), (_) => DbActionsEnabled);
    }

    /// <summary>
    /// Gets the list of confirmation options.
    /// </summary>
    public List<string> Confirmations { get; } = [string.Empty, "Yes, I confirm"];

    /// <summary>
    /// Gets a value indicating whether the database actions are enabled.
    /// </summary>
    public bool DbActionsEnabled => !string.IsNullOrEmpty(SelectedConfirmation) && !IsRunning;

    /// <summary>
    /// Gets the command to clear the database.
    /// </summary>
    public DelegateCommand ClearCommand { get; }

    /// <summary>
    /// Gets the command to fill the database.
    /// </summary>
    public DelegateCommand FillCommand { get; }

    private bool isRunning = false;

    /// <summary>
    /// Gets or sets a value indicating whether the database actions are running.
    /// </summary>
    public bool IsRunning
    {
        get => isRunning;
        set
        {
            if (value != isRunning)
            {
                isRunning = value;
                PropertyChanged?.Invoke(this, new(nameof(DbActionsEnabled)));
            }
        }
    }

    private string selectedConfirmation = string.Empty;

    /// <summary>
    /// Gets or sets the selected confirmation option.
    /// </summary>
    public string SelectedConfirmation
    {
        get => selectedConfirmation;
        set
        {
            if (value != selectedConfirmation)
            {
                selectedConfirmation = value;
                PropertyChanged?.Invoke(this, new(nameof(SelectedConfirmation)));
                PropertyChanged?.Invoke(this, new(nameof(DbActionsEnabled)));
            }
        }
    }

    private async void OnClear()
    {
        IsRunning = true;
        try
        {
            await writer.ClearAll();
            MessageBox.Show("DB has been cleared", "CCC", MessageBoxButton.OK);
        }
        finally
        {
            IsRunning = false;
        }
    }

    private async void OnFill()
    {
        IsRunning = true;
        try
        {
            await writer.Fill();
            MessageBox.Show("DB has been filled", "CCC", MessageBoxButton.OK);
        }
        finally
        {
            IsRunning = false;
        }
    }
}
