using System.Collections.ObjectModel;
using UniversityApp.Model.Entities;
using UniversityApp.ViewModel.ViewModels;

namespace UniversityApp.ViewModel.Models;

public class TreeItem : ViewModelBase
{
	public delegate Task TreeViewItemHasBeenExpandedHandler(object sender, EventArgs e);
	protected event TreeViewItemHasBeenExpandedHandler? TreeViewItemHasBeenExpanded;

	public string Name { get; set; }
	public string Tag { get; set; }

	public TreeItem? Parent { get; set; }

	private bool _isExpanded = false;
	public bool IsExpanded
	{
		get => _isExpanded;
		set
		{
			_isExpanded = value;
			if (value == true)
			{
                Task.Run(async () =>
				{
					if (TreeViewItemHasBeenExpanded != null)
					{
						await TreeViewItemHasBeenExpanded.Invoke(this, EventArgs.Empty);
                    }
				});
			}
			OnPropertyChanged();
		}
	}

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

	public TreeItem(
		string name, 
		string? tag = null,
		ObservableCollection<TreeItem>? children = null,
        TreeViewItemHasBeenExpandedHandler? onExpandedHandler = null,
		TreeItem? parent = null)
	{
		Name = name;
		Tag = tag == null ? string.Empty : tag;
		_children = children ?? new ObservableCollection<TreeItem>();
		if (onExpandedHandler != null)
		{
            TreeViewItemHasBeenExpanded += onExpandedHandler;
		}
		Parent = parent;
	}

}
