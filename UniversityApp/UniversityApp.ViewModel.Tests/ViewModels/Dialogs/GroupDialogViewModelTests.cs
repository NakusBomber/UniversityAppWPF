using UniversityApp.Model.Interfaces;
using UniversityApp.Model.Tests;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.ViewModel.Tests.ViewModels.Dialogs;

public class GroupDialogViewModelTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GroupDialogViewModel _vm;

    public GroupDialogViewModelTests()
    {
        _unitOfWork = new UnitOfWorkFake();
        _unitOfWork.TeacherRepository.Create(new Model.Entities.Teacher("FirstName", "SecondName"));
        _unitOfWork.TeacherRepository.Create(new Model.Entities.Teacher("One", "Two"));
        _unitOfWork.CourseRepository.Create(new Model.Entities.Course("Test1"));

        _vm = new GroupDialogViewModel(string.Empty, () => { }, _unitOfWork);
    }

    [Fact]
    public async Task GroupDialogViewModel_LoadAllDataCommand_Test()
    {
        var expected = true;
        await _vm.LoadAllDataCommand.ExecuteAsync(null);
        var actual = _vm.Courses.Any(c => c.Name == "Test1") &&
            _vm.Teachers.All(t => t.FirstName == "One" || t.FirstName == "FirstName");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GroupDialogViewModel_OkCommand_Test()
    {
        var expected = true;
        _vm.OkCommand.Execute(null);
        var actual = _vm.IsSuccess;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GroupDialogViewModel_CancelCommand_Test()
    {
        var expected = false;
        _vm.CancelCommand.Execute(null);
        var actual = _vm.IsSuccess;

        Assert.Equal(expected, actual);
    }
}
