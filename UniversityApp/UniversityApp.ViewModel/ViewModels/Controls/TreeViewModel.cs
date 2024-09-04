using System.Collections.ObjectModel;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Models;

namespace UniversityApp.ViewModel.ViewModels.Controls;

public class TreeViewModel : ViewModelBase
{
    private IUnitOfWork _unitOfWork;

    private ObservableCollection<TreeItem>? _items;
    public ObservableCollection<TreeItem> Items
    {
        get
        {
            if (_items == null)
            {
                _items = new ObservableCollection<TreeItem>();
            }
            return _items;
        }
        set
        {
            _items = value;
            OnPropertyChanged();
        }
    }

    public IAsyncCommand GetAllCoursesCommand { get; }

    public TreeViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        GetAllCoursesCommand = AsyncCommand.Create(ReloadTree);
    }

    private async Task ReloadTree(CancellationToken cancellationToken = default)
    {
        var courses = await GetCoursesAsync();

        // Includes
        await GetGroupsAsync();
        await GetStudentsAsync();

        Items = new ObservableCollection<TreeItem>(courses.Select(c => new TreeItem(c)));
    }

    private async Task<IEnumerable<Course>> GetCoursesAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        return await _unitOfWork.CourseRepository.GetAsync();
    }

    private async Task<IEnumerable<Group>> GetGroupsAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        return await _unitOfWork.GroupRepository.GetAsync();
    }

    private async Task<IEnumerable<Student>> GetStudentsAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        return await _unitOfWork.StudentRepository.GetAsync();
    }
}
