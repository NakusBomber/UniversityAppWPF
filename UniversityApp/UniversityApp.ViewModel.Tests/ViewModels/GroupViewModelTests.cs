using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;
using UniversityApp.Model.Tests;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels.Dialogs;
using UniversityApp.ViewModel.ViewModels.Pages;

namespace UniversityApp.ViewModel.Tests.ViewModels;

public class GroupViewModelTests
{
    private const string _dataForLineIterator = "StudentNumber,FirstName,LastName\r\n1,Emma,Grant  \r\n2,Mason,Brooks";
    private const string _nameGroupForCreate = "CreateGroup";
    private readonly Course _course = new Course("Course1");
    private readonly Teacher _teacher1 = new Teacher("Name1", "LastName1");
    private readonly Teacher _teacher2 = new Teacher("Name2", "LastName2");
    private readonly IUnitOfWork _unitOfWork;

    public GroupViewModelTests()
    {
        _unitOfWork = new UnitOfWorkFake();
        _unitOfWork.CourseRepository.Create(_course);
        _unitOfWork.TeacherRepository.Create(_teacher1);
        _unitOfWork.TeacherRepository.Create(_teacher2);
        var group1 = new Group("Group1", _course, _teacher1);
        var group2 = new Group("Group2", _course, _teacher2);
        _unitOfWork.GroupRepository.Create(group1);
        _unitOfWork.GroupRepository.Create(group2);
        _unitOfWork.StudentRepository.Create(new Student("Name1", "LastName1", group1));
        _unitOfWork.StudentRepository.Create(new Student("Name2", "LastName2", group1));
    }

    [Fact]
    public async Task GroupViewModel_OpenCreateGroupDialogCommand_CreateSuccess_Test()
    {
        var group = new Group(_nameGroupForCreate, _course, _teacher1);
        var vm = GetVMWithMock(group);
        await vm.OpenCreateGroupDialogCommand.ExecuteAsync(null);

        Assert.Contains(group, vm.Groups);
    }

    [Fact]
    public async Task GroupViewModel_OpenUpdateGroupDialogCommand_SelectedGroupNull_Test()
    {
        var vm = GetVMWithMock(new Group(string.Empty, _course, _teacher1));

        await vm.OpenUpdateGroupDialogCommand.ExecuteAsync(null);
        var isFaulted = vm.OpenUpdateGroupDialogCommand.Execution?.IsFaulted == true;
        var exception = vm.OpenUpdateGroupDialogCommand.Execution?.InnerException;

        Assert.True(isFaulted);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task GroupViewModel_OpenUpdateGroupDialogCommand_UpdateSuccess_Test()
    {
        var group = (await _unitOfWork.GroupRepository.GetAsync(g => g.Name == "Group1")).First();
        var expectedGroup = new Group(group.Id, "NewName", _course, _teacher2);
        var vm = GetVMWithMock(expectedGroup);
        vm.SelectedGroup = new Group(group.Id, group.Name!, _course, _teacher1);

        await vm.OpenUpdateGroupDialogCommand.ExecuteAsync(null);
        var expected = vm.Groups.First(g => g.Id == group.Id);
        Assert.True(expected.Name == "NewName");
        Assert.True(expected.Teacher == _teacher2);
    }

    [Fact]
    public async Task GroupViewModel_OpenUpdateGroupDialogCommand_UpdateCanceled_Test()
    {
        var group = (await _unitOfWork.GroupRepository.GetAsync(g => g.Name == "Group1")).First();
        var vm = GetVMWithMock();
        vm.SelectedGroup = new Group(group.Id, group.Name!, _course, _teacher1);

        await vm.OpenUpdateGroupDialogCommand.ExecuteAsync(null);

        var expected = await _unitOfWork.GroupRepository.GetByIdAsync(group.Id);
        Assert.Equal(expected, group);
    }

    [Fact]
    public async Task GroupViewModel_OpenUpdateGroupDialogCommand_OpenUpdateGroupDialogCommand_SelectedGroupCleared_Test()
    {
        var group = (await _unitOfWork.GroupRepository.GetAsync(g => g.Name == "Group1")).First();
        var expectedGroup = new Group(group.Id, "NewName", _course, _teacher2);
        var vm = GetVMWithMock(expectedGroup);
        vm.SelectedGroup = new Group(group.Id, group.Name!, _course, _teacher1);

        await vm.OpenUpdateGroupDialogCommand.ExecuteAsync(null);

        Assert.Null(vm.SelectedGroup);
    }

    [Fact]
    public async Task GroupViewModel_DeleteGroupCommand_SelectedGroupNull_Test()
    {
        var vm = GetVMWithMock(new Group(string.Empty, _course, _teacher1));

        await vm.DeleteGroupCommand.ExecuteAsync(null);
        var isFaulted = vm.DeleteGroupCommand.Execution?.IsFaulted == true;
        var exception = vm.DeleteGroupCommand.Execution?.InnerException;

        Assert.True(isFaulted);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task GroupViewModel_DeleteGroupCommand_Delete_Test()
    {
        var group = (await _unitOfWork.GroupRepository.GetAsync(g => g.Name == "Group1")).First();
        var vm = GetVMWithMock();
        vm.SelectedGroup = new Group(group.Id, group.Name!, _course, _teacher1);

        await vm.DeleteGroupCommand.ExecuteAsync(null);
        var value = vm.Groups.Any(g => g.Id == group.Id);
        Assert.False(value);
        Assert.True(vm.Groups.Count == 1);
    }

    [Fact]
    public async Task GroupViewModel_DeleteGroupCommand_RemoveStudentsFromGroup_Test()
    {
        var group = (await _unitOfWork.GroupRepository.GetAsync(g => g.Name == "Group1")).First();
        var vm = GetVMWithMock();
        vm.SelectedGroup = new Group(group.Id, group.Name!, _course, _teacher1);

        await vm.DeleteGroupCommand.ExecuteAsync(null);
        Assert.True((await _unitOfWork.StudentRepository.GetAsync()).All(s => s.Group == null));
    }

    [Fact]
    public async Task GroupViewModel_DeleteGroupCommand_SelectedGroupCleared_Test()
    {
        var group = (await _unitOfWork.GroupRepository.GetAsync(g => g.Name == "Group1")).First();
        var vm = GetVMWithMock();
        vm.SelectedGroup = new Group(group.Id, group.Name!, _course, _teacher1);

        await vm.DeleteGroupCommand.ExecuteAsync(null);

        Assert.Null(vm.SelectedGroup);
    }

    [Fact]
    public async Task GroupViewModel_ImportCommand_SelectedGroupNull_Test()
    {
        var vm = GetVMWithMock(new Group(string.Empty, _course, _teacher1));

        await vm.ImportCommand.ExecuteAsync(null);
        var isFaulted = vm.ImportCommand.Execution?.IsFaulted == true;
        var exception = vm.ImportCommand.Execution?.InnerException;

        Assert.True(isFaulted);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task GroupViewModel_ImportCommand_ImportSuccess_Test()
    {
        var group = (await _unitOfWork.GroupRepository.GetAsync(g => g.Name == "Group1")).First();
        var vm = GetVMWithMock();
        vm.SelectedGroup = group;

        await vm.ImportCommand.ExecuteAsync(null);
        var groupId = vm.Groups.First(g => g.Name == "Group1").Id;
        var actual1 = (await _unitOfWork.StudentRepository.GetAsync()).Any(s => s.FullName == "Emma Grant");
        var actual2 = (await _unitOfWork.StudentRepository.GetAsync()).Any(s => s.FullName == "Mason Brooks");
        Assert.True(actual1);
        Assert.True(actual2);
    }

    [Fact]
    public async Task GroupViewModel_ImportCommand_RemoveStudentsFromGroup_Test()
    {
        var group = (await _unitOfWork.GroupRepository.GetAsync(g => g.Name == "Group1")).First();
        var vm = GetVMWithMock();
        vm.SelectedGroup = group;

        await vm.ImportCommand.ExecuteAsync(null);
        var students = vm.Groups.First(g => g.Name == "Group1").Students;
        var actual1 = students.Any(s => s.FullName == "Name1 LastName1");
        var actual2 = students.Any(s => s.FullName == "Name2 LastName2");

        Assert.False(actual1);
        Assert.False(actual2);
    }

    [Fact]
    public async Task GroupViewModel_ReloadGroupsCommand_Test()
    {
        var vm = GetVMWithMock();
        var expected = await _unitOfWork.GroupRepository.GetAsync();
        await vm.ReloadGroupsCommand.ExecuteAsync(null);
        var actual = vm.Groups;

        Assert.Equal(expected, actual);
    }

    private GroupViewModel GetVMWithMock(Group? group = null)
    {
        var lineIterator = new LineIteratorStub(_dataForLineIterator.Split("\r\n"));
        var importer = new StudentImporter(lineIterator);
        var exporter = new StudentExporterFake();
        var mockGroup = new Mock<IWindowService<GroupDialogViewModel, GroupDialogResult>>();
        var result = new GroupDialogResult(group != null, group);
        mockGroup
            .Setup(mock => mock.Show(It.IsAny<GroupDialogViewModel>()))
            .Returns(result);
        var mockMessageBox = new Mock<IWindowService<MessageBoxViewModel>>();
        var mockExport = new Mock<IWindowService<ExportDialogViewModel>>();
        var resultOpenFile = new OpenFileDialogResult(true, "notEmpty");
        var mockOpenFile = new Mock<IWindowService<BasicDialogViewModel, OpenFileDialogResult>>();
        mockOpenFile
            .Setup(mock => mock.Show(It.IsAny<BasicDialogViewModel>()))
            .Returns(resultOpenFile);
        return new GroupViewModel(
            _unitOfWork,
            importer,
            exporter,
            mockGroup.Object,
            mockOpenFile.Object,
            mockExport.Object,
            mockMessageBox.Object
        );
    }
}
