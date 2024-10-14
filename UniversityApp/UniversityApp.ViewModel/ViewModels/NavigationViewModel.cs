using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Helpers;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Stores;
using UniversityApp.ViewModel.ViewModels.Pages;

namespace UniversityApp.ViewModel.ViewModels;

public class NavigationViewModel : ViewModelBase
{
	private readonly INavigationStore _navigationStore;

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

	public ICommand NavigateCommand { get; set; }

	
	public NavigationViewModel(INavigationStore navigationStore)
	{
		_navigationStore = navigationStore;
		NavigateCommand = new RelayCommand(ChangeVM);

		_currentVM = _navigationStore.GetViewModel(EPages.Show);
	}

    private void ChangeVM(object? obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        EPages pageNeeded = (EPages)obj;
        CurrentVM = _navigationStore.GetViewModel(pageNeeded);
    }

}
