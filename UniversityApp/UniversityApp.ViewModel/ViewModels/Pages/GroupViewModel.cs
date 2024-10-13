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
    private readonly IImporter<Student> _importer;
    private readonly IExporter<Student> _exporter;
	private readonly IWindowService<GroupDialogViewModel, GroupDialogResult> _groupDialogService;
	private readonly IWindowService<MessageBoxViewModel> _messageBoxService;
    private readonly IWindowService<BasicDialogViewModel, OpenFileDialogResult> _openFileDialogService;
    private readonly IWindowService<ExportDialogViewModel> _exportDialogService;

	private ObservableCollection<Group>? _groups;

	public ObservableCollection<Group> Groups
	{
		get
		{
			if (_groups == null)
			{
				_groups = new ObservableCollection<Group>();
			}
			return _groups;
		}
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
        IImporter<Student> studentImporter,
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
        OpenUpdateGroupDialogCommand = AsyncCommand.Create(OpenUpdateGroupDialogAsync, CanUpdateGroup);
		DeleteGroupCommand = AsyncCommand.Create(DeleteGroupAsync, CanDeleteGroup);
		ReloadGroupsCommand = AsyncCommand.Create(ReloadAllGroupsAsync);

        ImportCommand = AsyncCommand.Create(ImportStudentsAsync, CanImportExportStudents);
        ExportCommand = AsyncCommand.Create(ExportStudentsAsync, CanImportExportStudents);
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
        IEnumerable<Student> importedData = await _importer.ImportAsync(path);
        
        if(!importedData.Any())
        {
            return;
        }

        var group = await _unitOfWork.GroupRepository.GetByIdAsync(SelectedGroup.Id);
        await RemoveStudentsFromGroupAsync(group);
        
        foreach (Student student in importedData)
        {
            student.Group = group;
            await _unitOfWork.StudentRepository.CreateAsync(student);
        }
        await _unitOfWork.SaveAsync();
        await ReloadAllGroupsAsync();
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
    private bool CanImportExportStudents(object? parameter)
    {
        return SelectedGroup != null;
    }

    private async Task OpenCreateGroupDialogAsync(CancellationToken cancellationToken = default)
    {
        var newVM = new GroupDialogViewModel("Create group", CloseActiveWindow, _unitOfWork);

        GroupDialogResult result = _groupDialogService.Show(newVM);
        
        if (result.IsSuccess && result.Group != null)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
                await _unitOfWork.GroupRepository.CreateAsync(result.Group);
                await _unitOfWork.SaveAsync();
                await ReloadAllGroupsAsync();
            }
            catch (DbUpdateException)
            {
                await OpenErrorMessageBoxAsync("Already is a group by that name");
            }
            catch (Exception e)
            {
                await OpenErrorMessageBoxAsync(e.Message);
            }
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
            try
            {
                var group = await _unitOfWork.GroupRepository.GetByIdAsync(SelectedGroup.Id);
                group.Name = result.Group.Name;
                group.Course = await _unitOfWork.CourseRepository.GetByIdAsync(result.Group.Course!.Id);
                group.Teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(result.Group.Teacher!.Id);

                if (!Entity.AreEntitiesEqual(group, SelectedGroup))
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
                    await _unitOfWork.GroupRepository.UpdateAsync(group);
                    await _unitOfWork.SaveAsync();
                    SelectedGroup = null;
                    await ReloadAllGroupsAsync();
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                await OpenErrorMessageBoxAsync(e.Message);
            }
            catch (DbUpdateException)
            {
                await OpenErrorMessageBoxAsync("Already is a group by that name");
            }
            catch (Exception e)
            {
                await OpenErrorMessageBoxAsync(e.Message);
            }
        }
    }

    private bool CanUpdateGroup(object? parameter)
    {
        return SelectedGroup != null;
    }
    private async Task OpenErrorMessageBoxAsync(string message, CancellationToken cancellationToken = default)
    {
        var messageViewModel = new MessageBoxViewModel(
            "Error",
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
		await _unitOfWork.SaveAsync();

		SelectedGroup = null;
		await ReloadAllGroupsAsync();
	}

	private bool CanDeleteGroup(object? parameter)
	{
		return SelectedGroup != null;
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
        foreach (Student student in students)
        {
            student.Group = null;
            await _unitOfWork.StudentRepository.UpdateAsync(student);
        }
    }
}