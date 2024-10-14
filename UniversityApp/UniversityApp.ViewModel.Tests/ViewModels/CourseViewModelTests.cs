using Moq;
using System.Collections.ObjectModel;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.Model.Tests;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels.Dialogs;
using UniversityApp.ViewModel.ViewModels.Pages;

namespace UniversityApp.ViewModel.Tests.ViewModels;

public class CourseViewModelTests
{
    private const string _nameCourseForCreate = "CreateCourse";
    private readonly IUnitOfWork _unitOfWork;

    public CourseViewModelTests()
    {
        _unitOfWork = new UnitOfWorkFake();
        _unitOfWork.CourseRepository.Create(new Course("StartCourse1"));
        _unitOfWork.CourseRepository.Create(new Course("StartCourse2"));
    }

    [Fact]
    public async Task CourseViewModel_OpenCreateCourseDialogCommand_CreateSuccess_Test()
    {
        var course = new Course(_nameCourseForCreate);
        var vm = GetVMWithMock(course);
        await vm.OpenCreateCourseDialogCommand.ExecuteAsync(null);

        Assert.Contains(course, vm.Courses);
    }

    [Fact]
    public async Task CourseViewModel_OpenUpdateCourseDialogCommand_SelectedCourseNull_Test()
    {
        var vm = GetVMWithMock(new Course(string.Empty));

        await vm.OpenUpdateCourseDialogCommand.ExecuteAsync(null);
        var isFaulted = vm.OpenUpdateCourseDialogCommand.Execution?.IsFaulted == true;
        var exception = vm.OpenUpdateCourseDialogCommand.Execution?.InnerException;

        Assert.True(isFaulted);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task CourseViewModel_OpenUpdateCourseDialogCommand_UpdateSuccess_Test()
    {
        var course = (await _unitOfWork.CourseRepository.GetAsync(c => c.Name == "StartCourse1")).First();
        var expectedCourse = new Course(course.Id, "NewName", "New");
        var vm = GetVMWithMock(expectedCourse);
        vm.SelectedCourse = new Course(course.Id, course.Name, course.Description);

        await vm.OpenUpdateCourseDialogCommand.ExecuteAsync(null);
        var expected = vm.Courses.First(c => c.Id == course.Id);
        Assert.True(expected.Name == "NewName");
        Assert.True(expected.Description == "New");
    }

    [Fact]
    public async Task CourseViewModel_OpenUpdateCourseDialogCommand_UpdateCanceled_Test()
    {
        var course = (await _unitOfWork.CourseRepository.GetAsync(c => c.Name == "StartCourse1")).First();
        var vm = GetVMWithMock();
        vm.SelectedCourse = new Course(course.Id, course.Name, course.Description);

        await vm.OpenUpdateCourseDialogCommand.ExecuteAsync(null);

        var expected = await _unitOfWork.CourseRepository.GetByIdAsync(course.Id);
        Assert.Equal(expected, course);
    }

    [Fact]
    public async Task CourseViewModel_OpenUpdateCourseDialogCommand_SelectedCourseCleared_Test()
    {
        var course = (await _unitOfWork.CourseRepository.GetAsync(c => c.Name == "StartCourse1")).First();
        var newCourse = new Course(course.Id, "N", "D");
        var vm = GetVMWithMock(newCourse);
        vm.SelectedCourse = new Course(course.Id, course.Name, course.Description);

        await vm.OpenUpdateCourseDialogCommand.ExecuteAsync(null);

        Assert.Null(vm.SelectedCourse);
    }

    [Fact]
    public async Task CourseViewModel_DeleteCourseCommand_SelectedCourseNull_Test()
    {
        var vm = GetVMWithMock(new Course(string.Empty));

        await vm.DeleteCourseCommand.ExecuteAsync(null);
        var isFaulted = vm.DeleteCourseCommand.Execution?.IsFaulted == true;
        var exception = vm.DeleteCourseCommand.Execution?.InnerException;

        Assert.True(isFaulted);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task CourseViewModel_DeleteCourseCommand_Delete_Test()
    {
        var course = (await _unitOfWork.CourseRepository.GetAsync(c => c.Name == "StartCourse1")).First();
        var vm = GetVMWithMock();
        vm.SelectedCourse = course;

        await vm.DeleteCourseCommand.ExecuteAsync(null);
        var value = vm.Courses.Any(c => c.Name == course.Name);
        Assert.False(value);
        Assert.True(vm.Courses.Count == 1);
    }

    [Fact]
    public async Task CourseViewModel_DeleteCourseCommand_SelectedCourseCleared_Test()
    {
        var course = (await _unitOfWork.CourseRepository.GetAsync(c => c.Name == "StartCourse1")).First();
        var newCourse = new Course(course.Id, "NewData", "NewData");
        var vm = GetVMWithMock(newCourse);
        vm.SelectedCourse = new Course(course.Id, course.Name, course.Description);

        await vm.DeleteCourseCommand.ExecuteAsync(null);

        Assert.Null(vm.SelectedCourse);
    }

    [Fact]
    public async Task CourseViewModel_ReloadCoursesCommand_Test()
    {
        var vm = GetVMWithMock();
        var expected = await _unitOfWork.CourseRepository.GetAsync();
        await vm.ReloadCoursesCommand.ExecuteAsync(null);
        var actual = vm.Courses;

        Assert.Equal(expected, actual);
    }

    private CourseViewModel GetVMWithMock(Course? returnCourse = null)
    {
        var mockCourse = new Mock<IWindowService<CourseDialogViewModel, CourseDialogResult>>();
        var result = new CourseDialogResult(returnCourse != null, returnCourse);
        mockCourse
            .Setup(mock => mock.Show(It.IsAny<CourseDialogViewModel>()))
            .Returns(result);
        var mockMessageBox = new Mock<IWindowService<MessageBoxViewModel>>();

        return new CourseViewModel(_unitOfWork, mockCourse.Object, mockMessageBox.Object);
    }
}
