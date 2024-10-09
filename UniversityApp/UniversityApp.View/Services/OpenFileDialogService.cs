using Microsoft.Win32;
using System.Windows;
using UniversityApp.Model.Helpers;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.View.Services;

public class OpenFileDialogService : IWindowService<BasicDialogViewModel, OpenFileDialogResult>
{
    public OpenFileDialogResult Show(BasicDialogViewModel viewModel)
    {
        var dialog = new OpenFileDialog();
        dialog.Filter = EExportTypes.CSV.GetFilter();
        dialog.CheckPathExists = true;
        dialog.CheckFileExists = true;

        if(dialog.ShowDialog() != true)
        {
            return new OpenFileDialogResult(false);
        }

        return new OpenFileDialogResult(true, dialog.FileName);
    }

    public async Task<OpenFileDialogResult> ShowAsync(BasicDialogViewModel viewModel)
    {
        return await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            return Show(viewModel);
        });
    }
}
