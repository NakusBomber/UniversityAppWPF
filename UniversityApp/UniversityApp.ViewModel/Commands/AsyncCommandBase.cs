using System.Windows.Input;
using UniversityApp.ViewModel.Helpers;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.ViewModels;

namespace UniversityApp.ViewModel.Commands;

public abstract class AsyncCommandBase<TResult> :  ViewModelBase, IAsyncCommand<TResult>
{
    public abstract NotifyTaskCompletion<TResult>? Execution { get; protected set; }
    public abstract bool CanExecute(object? parameter);
    public abstract Task ExecuteAsync(object? parameter);
    public async void Execute(object? parameter)
    {
        await ExecuteAsync(parameter);
    }
    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
    protected void RaiseCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }
}
