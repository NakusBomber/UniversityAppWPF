using Ninject;
using UniversityApp.Model.Interfaces;

namespace UniversityApp.ViewModel.ViewModels.Pages;

public class StudentViewModel : ViewModelBase
{
    private IUnitOfWork _unitOfWork;

    [Inject]
    public StudentViewModel(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}
}
