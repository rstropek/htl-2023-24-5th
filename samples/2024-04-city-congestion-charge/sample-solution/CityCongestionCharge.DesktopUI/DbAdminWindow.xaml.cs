using System.Windows;

namespace CityCongestionCharge.DesktopUI;

public partial class DbAdminWindow : Window
{
    public DbAdminWindow(DbAdminWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
