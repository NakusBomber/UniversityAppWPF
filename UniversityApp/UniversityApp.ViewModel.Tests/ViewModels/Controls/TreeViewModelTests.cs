using System.Collections.ObjectModel;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;
using UniversityApp.Model.Tests;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels.Controls;

namespace UniversityApp.ViewModel.Tests.ViewModels.Controls;

public class TreeViewModelTests
{
    [Fact]
    public async Task TreeViewModel_ReloadCommand_NotEmptyCourses_Test()
    {
        var unitOfWork = new UnitOfWorkFake();
        unitOfWork.CourseRepository.Create(new Course("1"));
        unitOfWork.CourseRepository.Create(new Course("2"));
        var vm = new TreeViewModel(unitOfWork);

        var expected = 2;

        await vm.ReloadCoursesCommand.ExecuteAsync(null);
        var actual = vm.Items.Count;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task TreeViewModel_ReloadCommand_EmptyCourses_Test()
    {
        var unitOfWork = new UnitOfWorkFake();
        var vm = new TreeViewModel(unitOfWork);

        var expected = 0;

        await vm.ReloadCoursesCommand.ExecuteAsync(null);
        var actual = vm.Items.Count;

        Assert.Equal(expected, actual);
    }
}
