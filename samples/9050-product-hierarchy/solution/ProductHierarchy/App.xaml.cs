using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ProductHierarchy.Data;

namespace ProductHierarchy;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<MainWindow>();
        serviceCollection.AddSingleton<MainWindowViewModel>();

        var dbContextFactory = new ApplicationDbContextFactory();
        var dbContext = dbContextFactory.CreateDbContext(e.Args);
        serviceCollection.AddSingleton(dbContext);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        MainWindow = mainWindow;
    }
}

