using System.ComponentModel;
using System.Windows.Input;

namespace CashflowPlanner;

public class DelegateCommand : ICommand
{
    private readonly Action<object?> execute;
    private readonly Func<object?, bool>? canExecute;

    public DelegateCommand(INotifyPropertyChanged viewModel, Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
        viewModel.PropertyChanged += (sender, args) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => canExecute == null || canExecute(parameter);

    public void Execute(object? parameter) => execute(parameter);

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}