using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.Model.Tests;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.ViewModel.Tests.ViewModels.Dialogs;

public class StudentDialogViewModelTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly StudentDialogViewModel _vm;

    public StudentDialogViewModelTests()
    {
        _unitOfWork = new UnitOfWorkFake();
        var group = new Group("group", new Course(string.Empty), new Teacher("A", "B"));
        _unitOfWork.GroupRepository.Create(group);

        _vm = new StudentDialogViewModel(string.Empty, () => { }, _unitOfWork);
        _vm.Group = group;
    }

    [Fact]
    public void StudentDialogViewModel_ClearGroupCommand_Test()
    {
        _vm.ClearGroupCommand.Execute(null);
        var actual = _vm.Group;
        Assert.Null(actual);
    }

    [Fact]
    public async Task StudentDialogViewModel_LoadAllDataCommand_Test()
    {
        var expected = true;
        await _vm.LoadAllDataCommand.ExecuteAsync(null);
        var actual = _vm.Groups.Any(g => g.Name == "group");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void StudentDialogViewModel_OkCommand_Test()
    {
        var expected = true;
        _vm.OkCommand.Execute(null);
        var actual = _vm.IsSuccess;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void StudentDialogViewModel_CancelCommand_Test()
    {
        var expected = false;
        _vm.CancelCommand.Execute(null);
        var actual = _vm.IsSuccess;

        Assert.Equal(expected, actual);
    }
}
