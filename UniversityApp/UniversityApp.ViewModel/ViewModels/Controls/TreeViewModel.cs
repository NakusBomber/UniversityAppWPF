using System.Collections.ObjectModel;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Interfaces;

namespace UniversityApp.ViewModel.ViewModels.Controls;

public class TreeViewModel : ViewModelBase
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

    public TreeViewModel(
        IUnitOfWork unitOfWork,
        IAsyncCommand getAllCoursesCommand)
    {
        _unitOfWork = unitOfWork;
        GetAllCoursesCommand = getAllCoursesCommand;
    }

    private async Task GetAll(CancellationToken cancellationToken = default)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        var list = await _unitOfWork.CourseRepository.GetAsync();
        Courses = new ObservableCollection<Course>(list);
    }
}
