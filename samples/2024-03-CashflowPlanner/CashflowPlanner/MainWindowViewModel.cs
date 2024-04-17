namespace CashflowPlanner;

public class MainWindowViewModel
{
    public async Task LoadData()
    {
        // Load data from the database here
        throw new NotImplementedException();
    }

    // Note that the following list is just for demonstration purposes.
    // In your solution, you must calculate them yourself based on values
    // from the database.
    public List<PlannerLine> PlannerLines { get; } = [
        new(0, "Product Sales", 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000),
        new(0, "Service Revenue", 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200),
        new(0, "Interest Income", 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120),
        new(1, "Total Income", 1110, 2220, 3330, 4440, 5550, 6660, 7770, 8880, 9990, 11200, 12300, 12400),
        new(0, "Salaries and Wages", -1000, -2000, -3000, -4000, -5000, -6000, -7000, -8000, -9000, -10000, -11000, -12000),
        new(0, "Marketing and Advertising", -100, -200, -300, -400, -500, -600, -700, -800, -900, -1000, -1100, -1200),
        new(1, "Total Expenses", -1100, -1200, -1300, -1400, -1500, -1600, -1700, -1800, -1900, -2000, -2100, -2200),
        new(2, "Total", 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000, 15000, 16000),
    ];

}

public record PlannerLine(
    int Level, // 0 = regular, 1 = intermediate sum, 2 = final sum
    string CategoryName,
    decimal Jan,
    decimal Feb,
    decimal Mar,
    decimal Apr,
    decimal May,
    decimal Jun,
    decimal Jul,
    decimal Aug,
    decimal Sep,
    decimal Oct,
    decimal Nov,
    decimal Dec);