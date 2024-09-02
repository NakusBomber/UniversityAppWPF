using Ninject;
using UniversityApp.Model.Interfaces;

namespace UniversityApp.ViewModel.ViewModels.PagesVM;

public class CourseViewModel : ViewModelBase
{
    private IUnitOfWork _unitOfWork;

	[Inject]
	public CourseViewModel(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}
}
