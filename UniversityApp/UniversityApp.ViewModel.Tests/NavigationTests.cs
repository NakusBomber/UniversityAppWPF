using Moq;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;
using UniversityApp.Model.Tests;
using UniversityApp.ViewModel.Helpers;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.Stores;
using UniversityApp.ViewModel.ViewModels;
using UniversityApp.ViewModel.ViewModels.Dialogs;
using UniversityApp.ViewModel.ViewModels.Pages;
using Xunit.Sdk;

namespace UniversityApp.ViewModel.Tests;

public class NavigationTests
{
    private readonly IUnitOfWork _unitOfWork = new UnitOfWorkFake();
    private readonly ShowViewModel _showVM;
    private readonly CourseViewModel _courseVM;
    private readonly GroupViewModel _groupVM;
    private readonly StudentViewModel _studentVM;
    private readonly TeacherViewModel _teacherVM;

    private readonly NavigationStore _store;
    private readonly NavigationViewModel _navigationVM;

    public static IEnumerable<object[]> GetData()
    {
        return new List<object[]>
        {
            new object[] { EPages.Show, typeof(ShowViewModel) },
            new object[] { EPages.Courses, typeof(CourseViewModel) },
            new object[] { EPages.Groups, typeof(GroupViewModel) },
            new object[] { EPages.Students, typeof(StudentViewModel) },
            new object[] { EPages.Teachers, typeof(TeacherViewModel) },
        };
    }
    public NavigationTests()
    {
        var mockCourse = new Mock<IWindowService<CourseDialogViewModel, CourseDialogResult>>();
        var mockMessageBox = new Mock<IWindowService<MessageBoxViewModel>>();
        var mockGroup = new Mock<IWindowService<GroupDialogViewModel, GroupDialogResult>>();
        var mockOpenFile = new Mock<IWindowService<BasicDialogViewModel, OpenFileDialogResult>>();
        var mockExport = new Mock<IWindowService<ExportDialogViewModel>>();
        var mockStudent = new Mock<IWindowService<StudentDialogViewModel, StudentDialogResult>>();
        var mockTeacher = new Mock<IWindowService<TeacherDialogViewModel , TeacherDialogResult>>();

        _showVM = new ShowViewModel(_unitOfWork);
        _courseVM = new CourseViewModel(_unitOfWork, mockCourse.Object, mockMessageBox.Object);
        _groupVM = new GroupViewModel(
            _unitOfWork,
            new StudentImporter(),
            new StudentExporterFake(),
            mockGroup.Object,
            mockOpenFile.Object,
            mockExport.Object,
            mockMessageBox.Object);
        _studentVM = new StudentViewModel(_unitOfWork, mockMessageBox.Object, mockStudent.Object);
        _teacherVM = new TeacherViewModel(_unitOfWork, mockTeacher.Object, mockMessageBox.Object);

        _store = new NavigationStore(_showVM, _courseVM, _groupVM, _studentVM, _teacherVM);
        _navigationVM = new NavigationViewModel(_store);
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void NavigationStore_Data_Test(EPages value, Type expected)
    {
        var actual = _store.GetViewModel(value).GetType();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NavigationStore_InvalidData_Test()
    {
        object? value = 10;
        Assert.Throws<ArgumentException>(() => _store.GetViewModel((EPages)value));
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void NavigationViewModel_NavigateCommand_Data_Test(EPages value, Type expected)
    {
        _navigationVM.NavigateCommand.Execute(value);
        var actual = _navigationVM.CurrentVM.GetType();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NavigationViewModel_NavigateCommand_Null_Test()
    {
        Assert.Throws<ArgumentNullException>(() => _navigationVM.NavigateCommand.Execute(null));
    }

}
