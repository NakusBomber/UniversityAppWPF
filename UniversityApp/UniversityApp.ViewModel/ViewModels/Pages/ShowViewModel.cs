using Ninject;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.ViewModels.Controls;

namespace UniversityApp.ViewModel.ViewModels.Pages;

public class ShowViewModel : ViewModelBase
{
    private IUnitOfWork _unitOfWork;

    public TreeViewModel TreeViewModel { get; set; }

    [Inject]
    public ShowViewModel(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;

        TreeViewModel = new TreeViewModel(unitOfWork);
        if (TreeViewModel.ReloadCoursesCommand.CanExecute(this))
        {
            Task.Run(() => TreeViewModel.ReloadCoursesCommand.Execute(this));
        }
	}
}
