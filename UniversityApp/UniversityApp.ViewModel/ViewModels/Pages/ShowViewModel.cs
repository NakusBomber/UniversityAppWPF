using Ninject;
using UniversityApp.Model.Interfaces;

namespace UniversityApp.ViewModel.ViewModels.PagesVM;

public class ShowViewModel : ViewModelBase
{
    private IUnitOfWork _unitOfWork;

    [Inject]
    public ShowViewModel(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}
}
