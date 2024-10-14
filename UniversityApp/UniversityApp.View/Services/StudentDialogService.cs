using System.Windows;
using UniversityApp.Model.Entities;
using UniversityApp.View.Dialogs;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.View.Services;

public class StudentDialogService : IWindowService<StudentDialogViewModel, StudentDialogResult>
{
    public StudentDialogResult Show(StudentDialogViewModel viewModel)
    {
        var window = new StudentDialog();
        window.DataContext = viewModel;

        window.ShowDialog();

        var isSuccess = viewModel.IsSuccess;

        if (isSuccess && 
            !string.IsNullOrEmpty(viewModel.FirstName) && 
            !string.IsNullOrEmpty(viewModel.LastName))
        {
            return new StudentDialogResult(isSuccess, new Student(
                    viewModel.FirstName,
                    viewModel.LastName,
                    viewModel.Group
                ));
        }

        return new StudentDialogResult(false);
    }

    public async Task<StudentDialogResult> ShowAsync(StudentDialogViewModel viewModel)
    {
        return await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            return Show(viewModel);
        });
    }
}
