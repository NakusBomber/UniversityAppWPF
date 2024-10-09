using Microsoft.EntityFrameworkCore;
using Ninject;
using System.Collections.ObjectModel;
using System.Windows;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.ViewModel.ViewModels.Pages;

public class StudentViewModel : ViewModelBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWindowService<MessageBoxViewModel> _messageWindowService;
    private readonly IWindowService<StudentDialogViewModel, StudentDialogResult> _studentDialogService;

    private ObservableCollection<Student>? _students;
    public ObservableCollection<Student> Students
    {
        get
        {
            if (_students == null)
            {
                _students = new ObservableCollection<Student>();
            }
            return _students;
        }
        set
        {
            _students = value;
            OnPropertyChanged();
        }
    }

    private Student? _selectedStudent;

    public Student? SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            _selectedStudent = value;
            OnPropertyChanged();
        }
    }

    public IAsyncCommand<object?> LoadStudentsCommand { get; }
    public IAsyncCommand<object?> OpenCreateStudentDialogCommand { get; }
    public IAsyncCommand<object?> OpenUpdateStudentDialogCommand { get; }
    public IAsyncCommand<object?> DeleteStudentCommand { get; }

    [Inject]
    public StudentViewModel(
        IUnitOfWork unitOfWork,
        IWindowService<MessageBoxViewModel> messageWindowService,
        IWindowService<StudentDialogViewModel, StudentDialogResult> studentDialogService)
	{
		_unitOfWork = unitOfWork;
        _messageWindowService = messageWindowService;
        _studentDialogService = studentDialogService;

        LoadStudentsCommand = AsyncCommand.Create(ReloadAllStudentsAsync);
        OpenCreateStudentDialogCommand = AsyncCommand.Create(OpenCreateStudentDialogAsync);
        OpenUpdateStudentDialogCommand = AsyncCommand.Create(OpenUpdateStudentDialogAsync, CanUpdateStudent);
        DeleteStudentCommand = AsyncCommand.Create(DeleteStudentAsync, CanDeleteStudent);
	}

    private void CloseActiveWindow()
    {
        Application.Current.Windows.OfType<Window>().First(w => w.IsActive == true)?.Close();
    }

    private async Task OpenErrorMessageBoxAsync(string message, CancellationToken cancellationToken = default)
    {
        var messageViewModel = new MessageBoxViewModel(
            "Error",
            message,
            CloseActiveWindow
        );

        await _messageWindowService.ShowAsync(messageViewModel);
    }

    private async Task OpenCreateStudentDialogAsync(CancellationToken cancellationToken = default)
    {
        var newVM = new StudentDialogViewModel("Create student", CloseActiveWindow, _unitOfWork);

        StudentDialogResult result = _studentDialogService.Show(newVM);

        if (result.IsSuccess && result.Student != null)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
                await _unitOfWork.StudentRepository.CreateAsync(result.Student);
                await _unitOfWork.SaveAsync();

                await ReloadAllStudentsAsync();
            }
            catch (Exception e)
            {
                await OpenErrorMessageBoxAsync(e.Message);
            }
        }
    }

    private async Task OpenUpdateStudentDialogAsync(CancellationToken cancellationToken = default)
    {
        if (SelectedStudent == null)
        {
            throw new ArgumentNullException(nameof(SelectedStudent));
        }

        var newVM = new StudentDialogViewModel("Change student", CloseActiveWindow, _unitOfWork)
        {
            FirstName = SelectedStudent.FirstName ?? string.Empty,
            LastName = SelectedStudent.LastName ?? string.Empty,
            Group = SelectedStudent.Group,
        };

        StudentDialogResult result = _studentDialogService.Show(newVM);
        if (result.IsSuccess && result.Student != null)
        {
            try
            {
                var student = await _unitOfWork.StudentRepository.GetByIdAsync(SelectedStudent.Id);
                student.FirstName = result.Student.FirstName;
                student.LastName = result.Student.LastName;
                if(result.Student.Group != null)
                {
                    student.Group = await _unitOfWork.GroupRepository.GetByIdAsync(result.Student.Group.Id);
                }
                else
                {
                    student.Group = null;
                }

                if (!student.FullCompare(SelectedStudent))
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
                    await _unitOfWork.StudentRepository.UpdateAsync(student);
                    await _unitOfWork.SaveAsync();
                    
                    SelectedStudent = null;
                    await ReloadAllStudentsAsync();
                }
            }
            catch (Exception e)
            {
                await OpenErrorMessageBoxAsync(e.Message);
            }
        }
    }

    private bool CanUpdateStudent(object? parameter)
    {
        return SelectedStudent != null;
    }

    private async Task DeleteStudentAsync(CancellationToken cancellationToken = default)
    {
        if(SelectedStudent == null)
        {
            throw new ArgumentNullException(nameof(SelectedStudent));
        }

        var student = await _unitOfWork.StudentRepository.GetByIdAsync(SelectedStudent.Id);
        await _unitOfWork.StudentRepository.DeleteAsync(student);
        await _unitOfWork.SaveAsync();

        SelectedStudent = null;
        await ReloadAllStudentsAsync();
    }

    private bool CanDeleteStudent(object? parameter)
    {
        return SelectedStudent != null;
    }

    private async Task ReloadAllStudentsAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        var list = await _unitOfWork.StudentRepository.GetAsync(asNoTracking: true);
        Students = new ObservableCollection<Student>(list);
    }
}
