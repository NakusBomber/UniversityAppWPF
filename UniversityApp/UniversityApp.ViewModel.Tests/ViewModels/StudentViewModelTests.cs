using Moq;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.Model.Tests;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels.Dialogs;
using UniversityApp.ViewModel.ViewModels.Pages;

namespace UniversityApp.ViewModel.Tests.ViewModels;

public class StudentViewModelTests
{
    private const string _nameStudentForCreate = "Create";
    private const string _lastNameStudentForCreate = "Student";
    private readonly IUnitOfWork _unitOfWork;

    public StudentViewModelTests()
    {
        _unitOfWork = new UnitOfWorkFake();
        _unitOfWork.StudentRepository.Create(new Student("Name1", "LastName1"));
        _unitOfWork.StudentRepository.Create(new Student("Name2", "LastName2"));
        _unitOfWork.GroupRepository.Create(new Group("Group", new Course("course"), new Teacher("F", "L")));
    }

    [Fact]
    public async Task StudentViewModel_OpenCreateStudentDialogCommand_CreateSuccess_Test()
    {
        var student = new Student(_nameStudentForCreate, _lastNameStudentForCreate);
        var vm = GetVMWithMock(student);
        await vm.OpenCreateStudentDialogCommand.ExecuteAsync(null);

        Assert.Contains(student, vm.Students);
    }

    [Fact]
    public async Task StudentViewModel_OpenUpdateStudentDialogCommand_SelectedStudentNull_Test()
    {
        var vm = GetVMWithMock(new Student(string.Empty, string.Empty));

        await vm.OpenUpdateStudentDialogCommand.ExecuteAsync(null);
        var isFaulted = vm.OpenUpdateStudentDialogCommand.Execution?.IsFaulted == true;
        var exception = vm.OpenUpdateStudentDialogCommand.Execution?.InnerException;

        Assert.True(isFaulted);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task StudentViewModel_OpenUpdateStudentDialogCommand_UpdateSuccess_Test()
    {
        var student = (await _unitOfWork.StudentRepository.GetAsync(s => s.FullName == "Name1 LastName1")).First();
        var group = (await _unitOfWork.GroupRepository.GetAsync()).First();
        var expectedCourse = new Student(student.Id, "NewName", "NewLastName", group);
        var vm = GetVMWithMock(expectedCourse);
        vm.SelectedStudent = new Student(student.Id, student.FirstName!, student.LastName!, student.Group);

        await vm.OpenUpdateStudentDialogCommand.ExecuteAsync(null);
        var expected = vm.Students.First(s => s.Id == student.Id);
        Assert.True(expected.FirstName == "NewName");
        Assert.True(expected.LastName == "NewLastName");
        Assert.True(expected.Group == group);
    }

    [Fact]
    public async Task StudentViewModel_OpenUpdateStudentDialogCommand_UpdateCanceled_Test()
    {
        var student = (await _unitOfWork.StudentRepository.GetAsync(s => s.FullName == "Name1 LastName1")).First();
        var vm = GetVMWithMock();
        vm.SelectedStudent = new Student(student.Id, student.FirstName!, student.LastName!, student.Group);

        await vm.OpenUpdateStudentDialogCommand.ExecuteAsync(null);

        var expected = await _unitOfWork.StudentRepository.GetByIdAsync(student.Id);
        Assert.Equal(expected, student);
    }

    [Fact]
    public async Task StudentViewModel_OpenUpdateStudentDialogCommand_SelectedStudentCleared_Test()
    {
        var student = (await _unitOfWork.StudentRepository.GetAsync(s => s.FullName == "Name1 LastName1")).First();
        var group = (await _unitOfWork.GroupRepository.GetAsync()).First();
        var expectedCourse = new Student(student.Id, "NewName", "NewLastName", group);
        var vm = GetVMWithMock(expectedCourse);
        vm.SelectedStudent = new Student(student.Id, student.FirstName!, student.LastName!, student.Group);

        await vm.OpenUpdateStudentDialogCommand.ExecuteAsync(null);

        Assert.Null(vm.SelectedStudent);
    }

    [Fact]
    public async Task StudentViewModel_DeleteStudentCommand_SelectedStudentNull_Test()
    {
        var vm = GetVMWithMock(new Student(string.Empty, string.Empty));

        await vm.DeleteStudentCommand.ExecuteAsync(null);
        var isFaulted = vm.DeleteStudentCommand.Execution?.IsFaulted == true;
        var exception = vm.DeleteStudentCommand.Execution?.InnerException;

        Assert.True(isFaulted);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task StudentViewModel_DeleteStudentCommand_Delete_Test()
    {
        var student = (await _unitOfWork.StudentRepository.GetAsync(s => s.FullName == "Name1 LastName1")).First();
        var vm = GetVMWithMock();
        vm.SelectedStudent = student;

        await vm.DeleteStudentCommand.ExecuteAsync(null);
        var value = vm.Students.Any(s => s.Id == student.Id);
        Assert.False(value);
        Assert.True(vm.Students.Count == 1);
    }

    [Fact]
    public async Task StudentViewModel_DeleteStudentCommand_SelectedStudentCleared_Test()
    {
        var student = (await _unitOfWork.StudentRepository.GetAsync(s => s.FullName == "Name1 LastName1")).First();
        var vm = GetVMWithMock();
        vm.SelectedStudent = student;

        await vm.DeleteStudentCommand.ExecuteAsync(null);

        Assert.Null(vm.SelectedStudent);
    }

    [Fact]
    public async Task StudentViewModel_LoadStudentsCommand_Test()
    {
        var vm = GetVMWithMock();
        var expected = await _unitOfWork.StudentRepository.GetAsync();
        await vm.LoadStudentsCommand.ExecuteAsync(null);
        var actual = vm.Students;

        Assert.Equal(expected, actual);
    }

    private StudentViewModel GetVMWithMock(Student? returnStudent = null)
    {
        var mockStudent = new Mock<IWindowService<StudentDialogViewModel, StudentDialogResult>>();
        var result = new StudentDialogResult(returnStudent != null, returnStudent);
        mockStudent
            .Setup(mock => mock.Show(It.IsAny<StudentDialogViewModel>()))
            .Returns(result);
        var mockMessageBox = new Mock<IWindowService<MessageBoxViewModel>>();

        return new StudentViewModel(_unitOfWork, mockMessageBox.Object, mockStudent.Object);
    }
}

