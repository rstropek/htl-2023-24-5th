using System.Windows;

namespace Seats;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        DataContext = this.ViewModel = viewModel;
        InitializeComponent();
    }

    public MainWindowViewModel ViewModel { get; }
}