using System.Windows;
using UniversityApp.View.Dialogs;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.View.Services;

public class MessageBoxService : IWindowService<MessageBoxViewModel>
{
    public void Show(MessageBoxViewModel viewModel)
    {
        var window = new MyMessageBox();
        window.DataContext = viewModel;

        window.ShowDialog();
    }

    public async Task ShowAsync(MessageBoxViewModel viewModel)
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            Show(viewModel);
        });
    }
}
