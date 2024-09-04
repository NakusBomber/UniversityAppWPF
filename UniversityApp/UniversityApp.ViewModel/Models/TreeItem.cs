using System.Collections.ObjectModel;
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
		Children = children ?? new ObservableCollection<TreeItem>();
	}
}
