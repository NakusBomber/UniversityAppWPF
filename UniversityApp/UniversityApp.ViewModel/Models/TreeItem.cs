using System.Collections.ObjectModel;
using UniversityApp.Model.Entities;
using UniversityApp.ViewModel.ViewModels;

namespace UniversityApp.ViewModel.Models;

public class TreeItem : ViewModelBase
{
	public string Name { get; set; }

	private ObservableCollection<TreeItem> _children;
	public ObservableCollection<TreeItem> Children
	{
		get => _children;
		set
		{
			_children = value;
			OnPropertyChanged();
		}
	}

	public TreeItem(string name, ObservableCollection<TreeItem>? children = null)
	{
		Name = name;
		_children = children ?? new ObservableCollection<TreeItem>();
	}

	public TreeItem(Course course)
	{
		Name = course.Name ?? "";
		_children = new ObservableCollection<TreeItem>(
			course.Groups.Select(g => new TreeItem(g))
		);
	}

	public TreeItem(Group group)
	{
		Name = group.Name ?? "";
		_children = new ObservableCollection<TreeItem>(
			group.Students.Select(s => new TreeItem(s))
		);
	}

	public TreeItem(Student student)
	{
		Name = student.FullName;
		_children = new ObservableCollection<TreeItem>();
	}
}
