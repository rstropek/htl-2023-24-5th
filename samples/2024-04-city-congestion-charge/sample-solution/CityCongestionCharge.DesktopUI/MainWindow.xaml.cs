using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace CityCongestionCharge.DesktopUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainWindowViewModel ViewModel;
    private readonly IServiceProvider services;

    public MainWindow(MainWindowViewModel viewModel, IServiceProvider services)
    {
        InitializeComponent();
        DataContext = ViewModel = viewModel;

        // Note: Students have to know how to trigger data loading when window is loaded.
        Loaded += async (_, _) => await ViewModel.Refresh();
        this.services = services;
    }

    private void DbAdmin(object sender, RoutedEventArgs e)
    {
        // Opening a new window is done here, NOT in the view model.
        // The view model should not know about the window.
        var dbAdminWindow = services.GetRequiredService<DbAdminWindow>();
        dbAdminWindow.Owner = this;
        dbAdminWindow.ShowDialog();
    }
}
