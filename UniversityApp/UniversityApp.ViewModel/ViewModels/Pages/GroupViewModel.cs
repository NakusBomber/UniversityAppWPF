using Ninject;
using UniversityApp.Model.Interfaces;

namespace UniversityApp.ViewModel.ViewModels.PagesVM;

public class GroupViewModel : ViewModelBase
{
    private IUnitOfWork _unitOfWork;

	[Inject]
	public GroupViewModel(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}
}