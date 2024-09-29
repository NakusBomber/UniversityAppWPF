using System.Windows;
using UniversityApp.Model.Entities;
using UniversityApp.View.Dialogs;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.View.Services;

public class TeacherDialogService : IWindowService<TeacherDialogViewModel, TeacherDialogResult>
{
    public TeacherDialogResult Show(TeacherDialogViewModel viewModel)
    {
        var window = new TeacherDialog();
        window.DataContext = viewModel;

        window.ShowDialog();

        if (viewModel.IsSuccess &&
            !string.IsNullOrEmpty(viewModel.FirstName) &&
            !string.IsNullOrEmpty(viewModel.LastName))
        {
            return new TeacherDialogResult(true, new Teacher(viewModel.FirstName,viewModel.LastName));
        }

        return new TeacherDialogResult(false);
    }

    public async Task<TeacherDialogResult> ShowAsync(TeacherDialogViewModel viewModel)
    {
        return await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            return Show(viewModel);
        });
    }
}
