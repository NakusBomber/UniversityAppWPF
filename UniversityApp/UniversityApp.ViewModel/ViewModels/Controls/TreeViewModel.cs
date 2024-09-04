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
        var groups = await GetGroupsAsync();
        var students = await GetStudentsAsync();

        Items = new ObservableCollection<TreeItem>(from course in courses
                                                   select new TreeItem(course.Name ?? "",
                                                                       new ObservableCollection<TreeItem>(from gr in groups
                                                                                                          where gr.Course == course
                                                                                                          select new TreeItem(gr.Name ?? "",
                                                                                                                              new ObservableCollection<TreeItem>(from st in students
                                                                                                                                                                 where st.Group == gr
                                                                                                                                                                 select new TreeItem($"{st.FirstName} {st.LastName}"))))));
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
