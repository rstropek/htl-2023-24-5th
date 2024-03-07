using System.Windows;

namespace TaxiManager
{
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel ViewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = ViewModel = viewModel;
            Loaded += async (_, __) => await ViewModel.InitAsync();
        }
    }
}