using System.Windows.Input;

namespace UniversityApp.ViewModel.Interfaces;

public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync(object? parameter);
}
