using UniversityApp.ViewModel.Helpers;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.ViewModels;
using UniversityApp.ViewModel.ViewModels.Pages;

namespace UniversityApp.ViewModel.Stores;

public class NavigationStore : INavigationStore
{
    private readonly ShowViewModel _showVM;
    private readonly CourseViewModel _coursesVM;
    private readonly GroupViewModel _groupsVM;
    private readonly StudentViewModel _studentsVM;
    private readonly TeacherViewModel _teacherVM;

    public ViewModelBase GetViewModel(EPages page)
    {
        switch (page)
        {
            case EPages.Show:
                return _showVM;
            case EPages.Courses:
                return _coursesVM;
            case EPages.Groups:
                return _groupsVM;
            case EPages.Students:
                return _studentsVM;
            case EPages.Teachers:
                return _teacherVM;
            default:
                throw new ArgumentException(nameof(page));
        }
    }

    public NavigationStore(
        ShowViewModel showVM, 
        CourseViewModel coursesVM, 
        GroupViewModel groupsVM, 
        StudentViewModel studentsVM, 
        TeacherViewModel teacherVM)
    {
        _showVM = showVM;
        _coursesVM = coursesVM;
        _groupsVM = groupsVM;
        _studentsVM = studentsVM;
        _teacherVM = teacherVM;
    }
}
