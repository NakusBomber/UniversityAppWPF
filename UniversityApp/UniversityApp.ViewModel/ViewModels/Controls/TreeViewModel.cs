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
    private readonly IUnitOfWork _unitOfWork;
    private IEnumerable<Course>? _courses;
    private ObservableCollection<TreeItem> _fakeChildren
    {
        get => new ObservableCollection<TreeItem>
        {
            new TreeItem("...", "Fake")
        };
    }

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

    public IAsyncCommand ReloadCoursesCommand { get; }

    public TreeViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        ReloadCoursesCommand = AsyncCommand.Create(LoadCourses);
    }

    private async Task LoadCourses(CancellationToken cancellationToken = default)
    {
        _courses = await GetCoursesAsync();

        Items = new ObservableCollection<TreeItem>(
                _courses.Select(c => new TreeItem(
                    c.Name!, 
                    "Course", 
                    children: new ObservableCollection<TreeItem>(_fakeChildren),
                    onExpandedHandler: OnExpanded))
            );
    }

    private async Task<IEnumerable<Course>> GetCoursesAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        return await _unitOfWork.CourseRepository.GetAsync(asNoTracking: true);
    }
    private Task OnExpanded(object sender, EventArgs e)
    {
        var item = sender as TreeItem;
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        
        if (item.Tag == "Course")
        {
            OnExpandedCourse(item);
        }
        else if (item.Tag == "Group")
        {
            OnExpandedGroup(item);
        }

        return Task.CompletedTask;
    }

    private void OnExpandedCourse(TreeItem item)
    {
        if (_courses == null)
        {
            throw new ArgumentNullException(nameof(_courses));
        }

        Course? course = _courses.FirstOrDefault(c => c.Name == item.Name);
        if (course != null)
        {
            item.Children = new ObservableCollection<TreeItem>(
                course.Groups.Select(g => new TreeItem(
                    g.Name!,
                    "Group",
                    onExpandedHandler: OnExpanded,
                    children: _fakeChildren,
                    parent: item,
                    toolTipText: g.Teacher?.FullName))
            );
        }
    }

    private void OnExpandedGroup(TreeItem item)
    {
        if (_courses == null)
        {
            throw new ArgumentNullException(nameof(_courses));
        }

        Course? course = _courses.FirstOrDefault(c => c.Name == item.Parent!.Name);
        if (course == null)
        {
            throw new ArgumentNullException(nameof(course));
        }

        Group? group = course.Groups.FirstOrDefault(g => g.Name == item.Name);
        if (group != null)
        {
            item.Children = new ObservableCollection<TreeItem>(
                group.Students.Select(s => new TreeItem(
                    s.FullName,
                    "Student",
                    parent: item))
            );
        }
    }
  
}
