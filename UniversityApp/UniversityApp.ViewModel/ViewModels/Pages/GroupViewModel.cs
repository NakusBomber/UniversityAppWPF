using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Ninject;
using System.Collections.ObjectModel;
using System.Windows;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.ViewModel.ViewModels.Pages;

public class GroupViewModel : ViewModelBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImporter<StudentImportResult> _importer;
    private readonly IExporter<Student> _exporter;
	private readonly IWindowService<GroupDialogViewModel, GroupDialogResult> _groupDialogService;
	private readonly IWindowService<MessageBoxViewModel> _messageBoxService;
    private readonly IWindowService<BasicDialogViewModel, OpenFileDialogResult> _openFileDialogService;
    private readonly IWindowService<ExportDialogViewModel> _exportDialogService;

	private ObservableCollection<Group> _groups = new();

	public ObservableCollection<Group> Groups
	{
		get => _groups;
		set
		{
			_groups = value;
			OnPropertyChanged();
		}
	}

	private Group? _selectedGroup;

	public Group? SelectedGroup
	{
		get => _selectedGroup;
		set
		{
			_selectedGroup = value;
			OnPropertyChanged();
		}
	}

	public IAsyncCommand<object?> OpenCreateGroupDialogCommand { get; }
	public IAsyncCommand<object?> OpenUpdateGroupDialogCommand { get; }
	public IAsyncCommand<object?> DeleteGroupCommand { get; }
	public IAsyncCommand<object?> ReloadGroupsCommand { get; }
    public IAsyncCommand<object?> ImportCommand { get; }
    public IAsyncCommand<object?> ExportCommand { get; }

	[Inject]
	public GroupViewModel(
		IUnitOfWork unitOfWork,
        IImporter<StudentImportResult> studentImporter,
        IExporter<Student> studentExporter,
		IWindowService<GroupDialogViewModel, GroupDialogResult> groupDialogService,
        IWindowService<BasicDialogViewModel, OpenFileDialogResult> openFileDialogService,
        IWindowService<ExportDialogViewModel> exportDialogService,
        IWindowService<MessageBoxViewModel> messageBoxService)
	{
		_unitOfWork = unitOfWork;
        _importer = studentImporter;
        _exporter = studentExporter;
        _openFileDialogService = openFileDialogService;
        _exportDialogService = exportDialogService;
		_groupDialogService = groupDialogService;
		_messageBoxService = messageBoxService;

        OpenCreateGroupDialogCommand = AsyncCommand.Create(OpenCreateGroupDialogAsync);
        OpenUpdateGroupDialogCommand = AsyncCommand.Create(OpenUpdateGroupDialogAsync, IsGroupSelected);
		DeleteGroupCommand = AsyncCommand.Create(DeleteGroupAsync, IsGroupSelected);
		ReloadGroupsCommand = AsyncCommand.Create(ReloadAllGroupsAsync);

        ImportCommand = AsyncCommand.Create(ImportStudentsAsync, IsGroupSelected);
        ExportCommand = AsyncCommand.Create(ExportStudentsAsync, IsGroupSelected);
	}

    private async Task ImportStudentsAsync(CancellationToken cancellationToken = default)
    {
        if (SelectedGroup == null)
        {
            throw new ArgumentNullException(nameof(SelectedGroup));
        }
        var result = _openFileDialogService.Show(new BasicDialogViewModel(string.Empty));
        
        if(!result.IsSuccess || string.IsNullOrEmpty(result.FilePath))
        {
            return;
        }

        string path = result.FilePath;
        var importResult = await _importer.ImportAsync(path);
        
        if(!importResult.StudentsWithoutGroup.Any())
        {
            await OpenMessageBoxAsync("Info", "Not one student was imported");
            return;
        }

        var group = await _unitOfWork.GroupRepository.GetByIdAsync(SelectedGroup.Id);
        await RemoveStudentsFromGroupAsync(group);
        
        foreach (Student student in importResult.StudentsWithoutGroup)
        {
            student.Group = group;
            await _unitOfWork.StudentRepository.CreateAsync(student);
        }
        await SaveAndReloadAsync();

        var countStudents = importResult.StudentsWithoutGroup.Count();
        await OpenMessageBoxAsync("Info", $"Count imported students: {countStudents}\nCount line with error: {importResult.CountError}");
    }
    private async Task ExportStudentsAsync(CancellationToken cancellationToken = default)
    {
        if(SelectedGroup == null)
        {
            throw new ArgumentNullException(nameof(SelectedGroup));
        }

        var newVM = new ExportDialogViewModel(SelectedGroup, _exporter);
        await _exportDialogService.ShowAsync(newVM);
    }

    private async Task OpenCreateGroupDialogAsync(CancellationToken cancellationToken = default)
    {
        var newVM = new GroupDialogViewModel("Create group", CloseActiveWindow, _unitOfWork);

        GroupDialogResult result = _groupDialogService.Show(newVM);
        
        if (result.IsSuccess && result.Group != null)
        {
            await HandleDbExceptions(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
                await _unitOfWork.GroupRepository.CreateAsync(result.Group);
                await SaveAndReloadAsync();
            }, "Already is a group by that name");
        }
    }

    private async Task OpenUpdateGroupDialogAsync(CancellationToken cancellationToken = default)
    {
        if (SelectedGroup == null)
        {
            throw new ArgumentNullException(nameof(SelectedGroup));
        }

        var newVM = new GroupDialogViewModel("Change group", CloseActiveWindow, _unitOfWork)
        {
            Name = SelectedGroup!.Name ?? string.Empty,
            Course = SelectedGroup!.Course,
            Teacher = SelectedGroup!.Teacher,
        };

        GroupDialogResult result = _groupDialogService.Show(newVM);
        if (result.IsSuccess && result.Group != null)
        {
            await HandleDbExceptions(async () =>
            {
                var group = await _unitOfWork.GroupRepository.GetByIdAsync(SelectedGroup.Id);
                group.Name = result.Group.Name;
                group.Course = await _unitOfWork.CourseRepository.GetByIdAsync(result.Group.Course!.Id);
                group.Teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(result.Group.Teacher!.Id);

                if (!Entity.AreEntitiesEqual(group, SelectedGroup))
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
                    await _unitOfWork.GroupRepository.UpdateAsync(group);
                    await SaveAndReloadAsync();
                    SelectedGroup = null;
                }
            }, "Already is a group by that name");
        }
    }

    private async Task OpenErrorMessageBoxAsync(string message, CancellationToken cancellationToken = default)
    {
        await OpenMessageBoxAsync("Error", message, cancellationToken);
    }

    private async Task OpenMessageBoxAsync(string title, string message, CancellationToken cancellationToken = default)
    {
        var messageViewModel = new MessageBoxViewModel(
            title,
            message,
            CloseActiveWindow
        );

        await _messageBoxService.ShowAsync(messageViewModel);
    }
    private void CloseActiveWindow()
    {
        Application.Current.Windows.OfType<Window>().First(w => w.IsActive == true)?.Close();
    }

    private async Task DeleteGroupAsync(CancellationToken cancellationToken = default)
	{
		if (SelectedGroup == null)
		{
			throw new ArgumentNullException(nameof(SelectedGroup));
		}

        var group = await _unitOfWork.GroupRepository.GetByIdAsync(SelectedGroup.Id);
        await RemoveStudentsFromGroupAsync(group);
		await _unitOfWork.GroupRepository.DeleteAsync(group);
        await SaveAndReloadAsync();
		SelectedGroup = null;
	}

	private async Task ReloadAllGroupsAsync(CancellationToken cancellationToken = default)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		var list = await _unitOfWork.GroupRepository.GetAsync(asNoTracking: true);
		Groups = new ObservableCollection<Group>(list);
	}
    
    /// <summary>
    /// Remove students from group (without saving db)
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    private async Task RemoveStudentsFromGroupAsync(Group group)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        var students = await _unitOfWork.StudentRepository.GetAsync(s => s.GroupId == group.Id);
        students.ToList().ForEach(student => student.Group = null);
        await _unitOfWork.StudentRepository.UpdateRangeAsync(students);
    }

    private async Task HandleDbExceptions(Func<Task> action, string messageAlreadyExists)
    {
        try
        {
            await action();
        }
        catch (DbUpdateConcurrencyException e)
        {
            await OpenErrorMessageBoxAsync(e.Message);
        }
        catch (DbUpdateException)
        {
            await OpenErrorMessageBoxAsync(messageAlreadyExists);
        }
        catch (Exception e)
        {
            await OpenErrorMessageBoxAsync(e.Message);
        }
    }

    private async Task SaveAndReloadAsync(CancellationToken cancellationToken = default)
    {
        await _unitOfWork.SaveAsync();
        await ReloadAllGroupsAsync();
    }

    private bool IsGroupSelected(object? parameter) => SelectedGroup != null;
}