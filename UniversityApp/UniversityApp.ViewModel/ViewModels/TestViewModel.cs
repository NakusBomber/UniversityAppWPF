using System.Collections.ObjectModel;
using System.Windows.Input;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Interfaces;

namespace UniversityApp.ViewModel.ViewModels;

public class TestViewModel : ViewModelBase
{
    private IUnitOfWork _unitOfWork;

    private ObservableCollection<Course>? _courses;
    public ObservableCollection<Course> Courses
    {
        get
        {
            if (_courses == null)
            {
                _courses = new ObservableCollection<Course>();
            }
            return _courses;
        }
        set
        {
            _courses = value;
            OnPropertyChanged();
        }
    }

    public IAsyncCommand GetAllCoursesCommand { get; }

    public TestViewModel()
    {
        _unitOfWork = new UnitOfWork();
        GetAllCoursesCommand = AsyncCommand.Create(GetAll);
    }

    public TestViewModel(
        IUnitOfWork unitOfWork,
        IAsyncCommand getAllCoursesCommand)
    {
        _unitOfWork = unitOfWork;
        GetAllCoursesCommand = getAllCoursesCommand;
    }

    private async Task GetAll(CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.CourseRepository.GetAsync();
        cancellationToken.ThrowIfCancellationRequested();
        Courses = new ObservableCollection<Course>(list);
    }
}
