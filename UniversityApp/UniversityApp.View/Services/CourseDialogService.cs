using System.Windows;
using UniversityApp.Model.Entities;
using UniversityApp.View.Dialogs;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.View.Services;

public class CourseDialogService : IWindowService<CourseDialogViewModel, CourseDialogResult>
{
    public CourseDialogResult Show(CourseDialogViewModel viewModel)
    {
        var window = new CreateCourseDialog();
        window.DataContext = viewModel;

        window.ShowDialog();

        var isSuccess = viewModel.IsSuccess;
        
        if (isSuccess)
        {
            string? description = viewModel.Description == string.Empty ? null : viewModel.Description;
            var course = new Course(viewModel.Name, description);
            return new CourseDialogResult(true, course);
        }

        return new CourseDialogResult(false);
    }

    public async Task<CourseDialogResult> ShowAsync(CourseDialogViewModel viewModel)
    {
        return await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            return Show(viewModel);
        });
    }
}
