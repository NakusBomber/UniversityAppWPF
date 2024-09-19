using System.Windows;
using UniversityApp.View.Dialogs;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.View.Services;

public class ExportDialogService : IWindowService<ExportDialogViewModel>
{
    public void Show(ExportDialogViewModel viewModel)
    {
        var window = new ExportDialog();
        window.DataContext = viewModel;

        window.ShowDialog();
    }

    public async Task ShowAsync(ExportDialogViewModel viewModel)
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            Show(viewModel);
        });
    }
}
