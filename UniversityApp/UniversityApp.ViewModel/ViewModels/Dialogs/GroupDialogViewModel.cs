using System.Collections.ObjectModel;
using System.Windows.Input;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Interfaces;

namespace UniversityApp.ViewModel.ViewModels.Dialogs;

public class GroupDialogViewModel : ViewModelBase
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly Action _closeAction;

	public bool IsSuccess {  get; set; }

	private string _titleWindow;

	public string TitleWindow
	{
		get => _titleWindow;
		set
		{
			_titleWindow = value;
			OnPropertyChanged();
		}
	}


	private string _name;

	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			OnPropertyChanged();
		}
	}

	private Course? _course;

	public Course? Course
	{
		get => _course;
		set
		{
			_course = value;
			OnPropertyChanged();
		}
	}

	private Teacher? _teacher;

	public Teacher? Teacher
	{
		get => _teacher;
		set
		{
			_teacher = value;
			OnPropertyChanged();
		}
	}

	private ObservableCollection<Course>? _courses;

	public ObservableCollection<Course> Courses
	{
		get
		{
			if(_courses == null)
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

    private ObservableCollection<Teacher>? _teachers;

    public ObservableCollection<Teacher> Teachers
    {
        get
        {
            if (_teachers == null)
            {
                _teachers = new ObservableCollection<Teacher>();
            }
            return _teachers;
        }
        set
        {
            _teachers = value;
            OnPropertyChanged();
        }
    }

	public IAsyncCommand LoadAllDataCommand { get; }
    public ICommand OkCommand { get; }
	public ICommand CancelCommand { get; }

	public GroupDialogViewModel(
		string titleWindow,
		Action closeAction,
		IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_closeAction = closeAction;

		LoadAllDataCommand = AsyncCommand.Create(LoadAllDataAsync);

		OkCommand = new RelayCommand(OkClose);
		CancelCommand = new RelayCommand(CancelClose);
		IsSuccess = false;
		_titleWindow = titleWindow;
		_name = string.Empty;
	}

	private void OkClose(object? parameter)
	{
		IsSuccess = true;
		_closeAction?.Invoke();
	}

	private void CancelClose(object? parameter)
	{
		IsSuccess = false;
		_closeAction?.Invoke();
	}

	private async Task LoadCoursesAsync(CancellationToken cancellationToken = default)
	{
		Courses = new ObservableCollection<Course>();
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		Courses = new ObservableCollection<Course>(await _unitOfWork.CourseRepository.GetAsync());
	}

	private async Task LoadTeachersAsync(CancellationToken cancellationToken = default)
	{
		Teachers = new ObservableCollection<Teacher>();
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        Teachers = new ObservableCollection<Teacher>(await _unitOfWork.TeacherRepository.GetAsync());
	}

	private async Task LoadAllDataAsync(CancellationToken cancellationToken = default)
	{
		await LoadCoursesAsync(cancellationToken);
		await LoadTeachersAsync(cancellationToken);
	}
}
