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

public class GroupViewModel : ViewModelBase
{
    private readonly IUnitOfWork _unitOfWork;
	private readonly IWindowService<GroupDialogViewModel, GroupDialogResult> _groupDialogService;
	private readonly IWindowService<MessageBoxViewModel> _messageBoxService;

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

	public IAsyncCommand OpenCreateGroupDialogCommand { get; }
	public IAsyncCommand OpenUpdateGroupDialogCommand { get; }
	public IAsyncCommand DeleteGroupCommand { get; }
	public IAsyncCommand ReloadGroupsCommand { get; }

	[Inject]
	public GroupViewModel(
		IUnitOfWork unitOfWork,
		IWindowService<GroupDialogViewModel, GroupDialogResult> groupDialogService,
        IWindowService<MessageBoxViewModel> messageBoxService)
	{
		_unitOfWork = unitOfWork;
		_groupDialogService = groupDialogService;
		_messageBoxService = messageBoxService;

        OpenCreateGroupDialogCommand = AsyncCommand.Create(OpenCreateGroupDialogAsync);
        OpenUpdateGroupDialogCommand = new AsyncCommand<object?>(async _ =>
        {
            await OpenUpdateGroupDialogAsync();
            return null;
        }, CanUpdateGroup);
		DeleteGroupCommand = new AsyncCommand<object?>(async _ =>
		{
			await DeleteGroupAsync();
			return null;
		}, CanDeleteGroup);
		ReloadGroupsCommand = AsyncCommand.Create(ReloadAllGroupsAsync);
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
                var groups = await _unitOfWork.GroupRepository.GetAsync(g => g.Id == SelectedGroup.Id);
                var group = groups.FirstOrDefault();
                if(group == null)
                {
                    throw new ArgumentNullException(nameof(group));
                }
                group.Name = result.Group.Name;
                group.Course = (await _unitOfWork.CourseRepository.GetAsync(c => c.Id == result.Group.Course!.Id)).First();
                group.Teacher = (await _unitOfWork.TeacherRepository.GetAsync(t => t.Id == result.Group.Teacher!.Id)).First();

                if (!group.FullCompare(SelectedGroup))
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
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        foreach (Student student in SelectedGroup.Students)
        {
            student.Group = null;
            await _unitOfWork.StudentRepository.UpdateAsync(student);
        }
		await _unitOfWork.GroupRepository.DeleteAsync(SelectedGroup);
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
}