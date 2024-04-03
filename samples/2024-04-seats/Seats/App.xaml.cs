using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Seats
{
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

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            MainWindow = mainWindow;
        }
    }
}
