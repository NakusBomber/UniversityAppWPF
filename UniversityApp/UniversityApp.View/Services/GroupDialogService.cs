using System.Windows;
using UniversityApp.Model.Entities;
using UniversityApp.View.Dialogs;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.View.Services;

public class GroupDialogService : IWindowService<GroupDialogViewModel, GroupDialogResult>
{
    public GroupDialogResult Show(GroupDialogViewModel viewModel)
    {
        var window = new GroupDialog();
        window.DataContext = viewModel;

        window.ShowDialog();

        var isSuccess = viewModel.IsSuccess;

        if (isSuccess && viewModel.Course != null && viewModel.Teacher != null)
        {
            return new GroupDialogResult(true, new Group(
                    viewModel.Name,
                    viewModel.Course,
                    viewModel.Teacher
                ));
        }

        return new GroupDialogResult(false);
    }

    public async Task<GroupDialogResult> ShowAsync(GroupDialogViewModel viewModel)
    {
        return await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            return Show(viewModel);
        });
    }
}
