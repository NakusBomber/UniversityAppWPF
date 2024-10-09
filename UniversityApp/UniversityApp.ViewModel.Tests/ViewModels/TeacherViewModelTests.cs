using Moq;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.Model.Tests;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels.Dialogs;
using UniversityApp.ViewModel.ViewModels.Pages;

namespace UniversityApp.ViewModel.Tests.ViewModels;

public class TeacherViewModelTests
{
    private const string _nameTeacherForCreate = "Create";
    private const string _lastNameTeacherForCreate = "Teacher";
    private readonly IUnitOfWork _unitOfWork;

    public TeacherViewModelTests()
    {
        _unitOfWork = new UnitOfWorkFake();
        _unitOfWork.TeacherRepository.Create(new Teacher("Name1", "LastName1"));
        _unitOfWork.TeacherRepository.Create(new Teacher("Name2", "LastName2"));
    }

    [Fact]
    public async Task TeacherViewModel_OpenCreateTeacherCommand_CreateSuccess_Test()
    {
        var teacher = new Teacher(_nameTeacherForCreate, _lastNameTeacherForCreate);
        var vm = GetVMWithMock(teacher);
        await vm.OpenCreateTeacherCommand.ExecuteAsync(null);

        Assert.Contains(teacher, vm.Teachers);
    }

    [Fact]
    public async Task TeacherViewModel_OpenUpdateTeacherCommand_SelectedTeacherNull_Test()
    {
        var vm = GetVMWithMock(new Teacher(string.Empty, string.Empty));

        await vm.OpenUpdateTeacherCommand.ExecuteAsync(null);
        var isFaulted = vm.OpenUpdateTeacherCommand.Execution?.IsFaulted == true;
        var exception = vm.OpenUpdateTeacherCommand.Execution?.InnerException;

        Assert.True(isFaulted);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task TeacherViewModel_OpenUpdateTeacherCommand_UpdateSuccess_Test()
    {
        var teacher = (await _unitOfWork.TeacherRepository.GetAsync(s => s.FullName == "Name1 LastName1")).First();
        var expectedTeacher = new Teacher(teacher.Id, "NewName", "NewLastName");
        var vm = GetVMWithMock(expectedTeacher);
        vm.SelectedTeacher = new Teacher(teacher.Id, teacher.FirstName!, teacher.LastName!);

        await vm.OpenUpdateTeacherCommand.ExecuteAsync(null);
        var expected = vm.Teachers.First(t => t.Id == teacher.Id);
        Assert.True(expected.FirstName == "NewName");
        Assert.True(expected.LastName == "NewLastName");
    }

    [Fact]
    public async Task TeacherViewModel_OpenUpdateTeacherCommand_UpdateCanceled_Test()
    {
        var teacher = (await _unitOfWork.TeacherRepository.GetAsync(s => s.FullName == "Name1 LastName1")).First();
        var vm = GetVMWithMock();
        vm.SelectedTeacher = new Teacher(teacher.Id, teacher.FirstName!, teacher.LastName!);

        await vm.OpenUpdateTeacherCommand.ExecuteAsync(null);

        var expected = await _unitOfWork.TeacherRepository.GetByIdAsync(teacher.Id);
        Assert.Equal(expected, teacher);
    }

    [Fact]
    public async Task TeacherViewModel_OpenUpdateTeacherCommand_SelectedTeacherCleared_Test()
    {
        var teacher = (await _unitOfWork.TeacherRepository.GetAsync(s => s.FullName == "Name1 LastName1")).First();
        var expectedTeacherr = new Teacher(teacher.Id, "NewName", "NewLastName");
        var vm = GetVMWithMock(expectedTeacherr);
        vm.SelectedTeacher = new Teacher(teacher.Id, teacher.FirstName!, teacher.LastName!);

        await vm.OpenUpdateTeacherCommand.ExecuteAsync(null);

        Assert.Null(vm.SelectedTeacher);
    }

    [Fact]
    public async Task TeacherViewModel_DeleteTeacherCommand_SelectedTeacherNull_Test()
    {
        var vm = GetVMWithMock(new Teacher(string.Empty, string.Empty));

        await vm.DeleteTeacherCommand.ExecuteAsync(null);
        var isFaulted = vm.DeleteTeacherCommand.Execution?.IsFaulted == true;
        var exception = vm.DeleteTeacherCommand.Execution?.InnerException;

        Assert.True(isFaulted);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task TeacherViewModel_DeleteTeacherCommand_Delete_Test()
    {
        var teacher = (await _unitOfWork.TeacherRepository.GetAsync(s => s.FullName == "Name1 LastName1")).First();
        var vm = GetVMWithMock();
        vm.SelectedTeacher = teacher;

        await vm.DeleteTeacherCommand.ExecuteAsync(null);
        var value = vm.Teachers.Any(t => t.Id == teacher.Id);
        Assert.False(value);
        Assert.True(vm.Teachers.Count == 1);
    }

    [Fact]
    public async Task TeacherViewModel_DeleteTeacherCommand_SelectedTeacherCleared_Test()
    {
        var teacher = (await _unitOfWork.TeacherRepository.GetAsync(s => s.FullName == "Name1 LastName1")).First();
        var vm = GetVMWithMock();
        vm.SelectedTeacher = teacher;

        await vm.DeleteTeacherCommand.ExecuteAsync(null);

        Assert.Null(vm.SelectedTeacher);
    }

    [Fact]
    public async Task TeacherViewModel_LoadTeachersCommand_Test()
    {
        var vm = GetVMWithMock();
        var expected = await _unitOfWork.TeacherRepository.GetAsync();
        await vm.LoadTeachersCommand.ExecuteAsync(null);
        var actual = vm.Teachers;

        Assert.Equal(expected, actual);
    }

    private TeacherViewModel GetVMWithMock(Teacher? returnTeacher = null)
    {
        var mockTeacher = new Mock<IWindowService<TeacherDialogViewModel, TeacherDialogResult>>();
        var result = new TeacherDialogResult(returnTeacher != null, returnTeacher);
        mockTeacher
            .Setup(mock => mock.Show(It.IsAny<TeacherDialogViewModel>()))
            .Returns(result);
        var mockMessageBox = new Mock<IWindowService<MessageBoxViewModel>>();

        return new TeacherViewModel(_unitOfWork, mockTeacher.Object, mockMessageBox.Object);
    }
}
