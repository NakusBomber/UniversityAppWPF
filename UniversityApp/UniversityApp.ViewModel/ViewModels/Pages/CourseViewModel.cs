using Microsoft.EntityFrameworkCore;
using Ninject;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.ViewModel.ViewModels.Pages;

public class CourseViewModel : ViewModelBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWindowService<CreateCourseDialogViewModel, CreateCourseDialogResult> _createCourseDialogService;
    private readonly IWindowService<MessageBoxViewModel> _messageBoxService;

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

    private Course? _selectedCourse;
    public Course? SelectedCourse
    {
        get => _selectedCourse;
        set
        {
            _selectedCourse = value;
            OnPropertyChanged();
        }
    }

    public IAsyncCommand OpenCreateCourseDialogCommand {  get; set; }
    public IAsyncCommand DeleteCourseCommand { get; set; }
    public IAsyncCommand ReloadCoursesCommand {  get; set; }

    [Inject]
    public CourseViewModel(
        IUnitOfWork unitOfWork,
        IWindowService<CreateCourseDialogViewModel,CreateCourseDialogResult> createCourseDialogService,
        IWindowService<MessageBoxViewModel> messageBoxService)
    {
        _unitOfWork = unitOfWork;
        _createCourseDialogService = createCourseDialogService;
        _messageBoxService = messageBoxService;

        OpenCreateCourseDialogCommand = AsyncCommand.Create(OpenCreateCourseDialogAsync);
        DeleteCourseCommand = new AsyncCommand<object?>(async _ =>
        {
            await DeleteCourseAsync();
            return null;
        }, CanDeleteCourse);
        ReloadCoursesCommand = AsyncCommand.Create(GetAllCoursesAsync);
    }

    private async Task OpenCreateCourseDialogAsync(CancellationToken cancellationToken = default)
    {
        var newVM = new CreateCourseDialogViewModel(() =>
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive == true)?.Close();
        });

        CreateCourseDialogResult result = _createCourseDialogService.Show(newVM);
        if (result.IsSuccess && result.Course != null)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
                await _unitOfWork.CourseRepository.CreateAsync(result.Course);
                await _unitOfWork.SaveAsync();
                await GetAllCoursesAsync();
            }
            catch (DbUpdateException)
            {
                var messageViewModel = new MessageBoxViewModel(
                    "Error",
                    "Already is a course by that name",
                    () =>
                    {
                        Application.Current.Windows.OfType<Window>().First(w => w.IsActive == true)?.Close();
                    });
                await _messageBoxService.ShowAsync(messageViewModel);
            }
            catch (Exception e)
            {
                var messageViewModel = new MessageBoxViewModel(
                    "Error",
                    e.Message,
                    () =>
                    {
                        Application.Current.Windows.OfType<Window>().First(w => w.IsActive == true)?.Close();
                    });
                await _messageBoxService.ShowAsync(messageViewModel);
            }
        }
    }

    private async Task DeleteCourseAsync(CancellationToken cancellationToken = default)
    {
        if (SelectedCourse == null)
        {
            throw new ArgumentNullException(nameof(SelectedCourse));
        }
        await _unitOfWork.CourseRepository.DeleteAsync(SelectedCourse);
        await _unitOfWork.SaveAsync();
        SelectedCourse = null;
        await GetAllCoursesAsync();
    }

    private bool CanDeleteCourse(object? parameter)
    {
        return SelectedCourse != null && SelectedCourse.Groups.Count == 0;
    }

    private async Task GetAllCoursesAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        var list = await _unitOfWork.CourseRepository.GetAsync();
        Courses = new ObservableCollection<Course>(list);
    }
}
