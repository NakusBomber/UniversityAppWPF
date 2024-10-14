using UniversityApp.ViewModel.Helpers;
using UniversityApp.ViewModel.ViewModels;

namespace UniversityApp.ViewModel.Interfaces;

public interface INavigationStore
{
    ViewModelBase GetViewModel(EPages page);
}
