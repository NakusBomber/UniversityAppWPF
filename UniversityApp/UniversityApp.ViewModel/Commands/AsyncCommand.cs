using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UniversityApp.ViewModel.Helpers;

namespace UniversityApp.ViewModel.Commands;

// Reference: https://learn.microsoft.com/ru-ru/archive/msdn-magazine/2014/april/async-programming-patterns-for-asynchronous-mvvm-applications-commands
public class AsyncCommand<TResult> : AsyncCommandBase<TResult>
{
    private readonly Func<CancellationToken, Task<TResult>> _command;
    private readonly Func<object?, bool>? _canExecute;
    private readonly CancelAsyncCommand _cancelCommand;
    private NotifyTaskCompletion<TResult>? _execution;

    public AsyncCommand(
        Func<CancellationToken, Task<TResult>> command,
        Func<object?, bool>? canExecute = null)
    {
        _command = command;
        _canExecute = canExecute;
        _cancelCommand = new CancelAsyncCommand();
    }

    public override bool CanExecute(object? parameter)
    {
        return (_canExecute == null || _canExecute(parameter)) && 
            (Execution == null || Execution.IsCompleted);
    }

    public override async Task ExecuteAsync(object? parameter)
    {
        _cancelCommand.NotifyCommandStarting();
        Execution = new NotifyTaskCompletion<TResult>(_command(_cancelCommand.Token));
        RaiseCanExecuteChanged();
        await Execution.TaskCompletion;
        _cancelCommand.NotifyCommandFinished();
        RaiseCanExecuteChanged();
    }

    public ICommand CancelCommand
    {
        get { return _cancelCommand; }
    }

    public override NotifyTaskCompletion<TResult>? Execution
    {
        get { return _execution; }
        protected set
        {
            _execution = value;
            OnPropertyChanged();
        }
    }
}

public static class AsyncCommand
{
    public static AsyncCommand<object?> Create(Func<Task> command, Func<object?, bool>? canExecute = null)
    {
        return new AsyncCommand<object?>(async _ => { await command(); return null; }, canExecute);
    }

    public static AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command)
    {
        return new AsyncCommand<TResult>(_ => command());
    }

    public static AsyncCommand<object?> Create(Func<CancellationToken, Task> command, Func<object?, bool>? canExecute = null)
    {
        return new AsyncCommand<object?>(async token => { await command(token); return null; }, canExecute);
    }

    public static AsyncCommand<TResult> Create<TResult>(Func<CancellationToken, Task<TResult>> command)
    {
        return new AsyncCommand<TResult>(command);
    }
}