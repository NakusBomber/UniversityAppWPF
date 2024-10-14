using Ninject;
using System.Windows.Input;
using System.Windows.Threading;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.ViewModels.Controls;

namespace UniversityApp.ViewModel.ViewModels.Pages;

public class ShowViewModel : ViewModelBase
{
    private readonly IUnitOfWork _unitOfWork;

    public TreeViewModel TreeViewModel { get; set; }

    [Inject]
    public ShowViewModel(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;

        TreeViewModel = new TreeViewModel(unitOfWork);
	}
}
