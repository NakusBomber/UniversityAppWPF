using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.ViewModels.PagesVM;

namespace UniversityApp.ViewModel.ViewModels;

public class NavigationViewModel : ViewModelBase
{
	private ViewModelBase _currentVM;
	public ViewModelBase CurrentVM
	{
		get => _currentVM;
		set
		{
            _currentVM = value;
			OnPropertyChanged();
		}
	}

	private ObservableCollection<ViewModelBase> _viewModels;

	public ObservableCollection<ViewModelBase> ViewModels
	{
		get => _viewModels;
		set
		{
			_viewModels = value;
			OnPropertyChanged();
		}
	}


	public ICommand NavigateCommand { get; set; }

	private void ChangeVM(object? obj)
	{
        ArgumentNullException.ThrowIfNull(obj);

		string vmName = (string)obj;
		if (vmName == null || vmName.Length == 0)
		{
			throw new ArgumentException("Incorrect name VM");
		}

		ViewModelBase vm;
		switch (vmName)
		{
			case "Show":
				vm = _kernel.Get<ShowViewModel>(); // ?
				break;
		}
	}
	public NavigationViewModel()
	{
		//NavigateShowCommand = new RelayCommand(_ => CurrentPage = new ShowPage());
	}
}
