using System.Collections.ObjectModel;
using System.Windows.Input;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Validations;

namespace UniversityApp.ViewModel.ViewModels.Dialogs;

public class GroupDialogViewModel : BasicDialogViewModel
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly Action _closeAction;

	public bool IsSuccess {  get; set; }

	private string _name;

	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			ValidateName();
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
			ValidateCourse();
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
            ValidateTeacher();
            OnPropertyChanged();
		}
	}

	private ObservableCollection<Course> _courses = new();

	public ObservableCollection<Course> Courses
	{
		get => _courses;
		set
		{
			_courses = value;
			OnPropertyChanged();
		}
	}

    private ObservableCollection<Teacher> _teachers = new();

    public ObservableCollection<Teacher> Teachers
    {
		get => _teachers;
        set
        {
            _teachers = value;
            OnPropertyChanged();
        }
    }

	public IAsyncCommand<object?> LoadAllDataCommand { get; }
    public ICommand OkCommand { get; }
	public ICommand CancelCommand { get; }

	public GroupDialogViewModel(
		string titleWindow,
		Action closeAction,
		IUnitOfWork unitOfWork)
		: base(titleWindow)
	{
		_unitOfWork = unitOfWork;
		_closeAction = closeAction;

		LoadAllDataCommand = AsyncCommand.Create(LoadAllDataAsync);

		OkCommand = new RelayCommand(OkClose, CanOk);
		CancelCommand = new RelayCommand(CancelClose);
		IsSuccess = false;
		_name = string.Empty;

		ValidateAll();
	}
	private void ValidateAll()
	{
		ValidateName();
		ValidateCourse();
		ValidateTeacher(); 
    }
	private void ValidateName() => Validate(_name, new EntityNameValidationRule(1, 50), nameof(Name));
	private void ValidateTeacher() => Validate<NotNullValidationRule>(_teacher, nameof(Teacher));
	private void ValidateCourse() => Validate<NotNullValidationRule>(_course, nameof(Course));

    private bool CanOk(object? arg)
    {
		return !HasErrors;
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
