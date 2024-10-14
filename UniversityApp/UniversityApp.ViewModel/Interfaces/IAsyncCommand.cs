using System.Windows.Input;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Helpers;

namespace UniversityApp.ViewModel.Interfaces;

public interface IAsyncCommand<TResult> : ICommand
{
    Task ExecuteAsync(object? parameter);
    NotifyTaskCompletion<TResult>? Execution { get; }
}