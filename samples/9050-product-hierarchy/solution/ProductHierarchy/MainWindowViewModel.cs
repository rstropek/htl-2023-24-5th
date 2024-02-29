using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using ProductHierarchy.Data;

namespace ProductHierarchy;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public MainWindowViewModel(ProductionDataContext dbContext)
    {
        SearchCommand = new DelegateCommand(this, async _ =>
        {
            try
            {
                var calc = new ProductPriceCalculator();
                var products = await calc.CalculateProductPriceAsync(dbContext, ProductID);
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        });
    }

    public ICommand SearchCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<ProductPriceCalculator.ProductAmount> Products { get; set; } = [];

    public int ProductID { get; set; } = 904;
}
