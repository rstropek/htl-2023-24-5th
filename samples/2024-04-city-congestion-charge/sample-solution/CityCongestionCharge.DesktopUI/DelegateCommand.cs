using System;
using System.ComponentModel;
using System.Windows.Input;

namespace CityCongestionCharge.DesktopUI;

/// <summary>
/// Represents a command that delegates the execution logic to the specified methods.
/// </summary>
public class DelegateCommand : ICommand
{
    private readonly Action<object?> execute;
    private readonly Func<object?, bool>? canExecute;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
    /// </summary>
    /// <param name="viewModel">The view model that contains the command.</param>
    /// <param name="execute">The method to be called when the command is executed.</param>
    /// <param name="canExecute">The method that determines whether the command can be executed.</param>
    /// <remarks>
    /// CanExecute will be re-evaluated when the view model raises the PropertyChanged event.
    /// </remarks>
    public DelegateCommand(INotifyPropertyChanged viewModel, Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
        viewModel.PropertyChanged += (sender, args) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
    /// </summary>
    /// <param name="execute">The method to be called when the command is executed.</param>
    /// <param name="canExecute">The method that determines whether the command can be executed.</param>
    public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    /// <summary>
    /// Occurs when changes occur that affect whether the command should execute.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">Data used by the command.</param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public bool CanExecute(object? parameter) => canExecute == null || canExecute(parameter);

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter">Data used by the command.</param>
    public void Execute(object? parameter) => execute(parameter);

    /// <summary>
    /// Raises the <see cref="CanExecuteChanged"/> event.
    /// </summary>
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
