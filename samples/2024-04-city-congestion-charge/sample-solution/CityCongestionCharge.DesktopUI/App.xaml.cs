using CityCongestionCharge.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace CityCongestionCharge.DesktopUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();

        var dbContextFactory = new CccDataContextFactory();
        var dbContext = dbContextFactory.CreateDbContext(e.Args);
        serviceCollection.AddSingleton(dbContext);
        serviceCollection.AddSingleton<MainWindow>();
        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection.AddSingleton<DemoDataWriter>();
        serviceCollection.AddSingleton<DemoDataGenerator>();
        serviceCollection.AddTransient<DbAdminWindow>();
        serviceCollection.AddTransient<DbAdminWindowViewModel>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        MainWindow = mainWindow;
    }
}
