using Ninject;
using UniversityApp.Model.Interfaces;

namespace UniversityApp.ViewModel.ViewModels.Pages;

public class TeacherViewModel : ViewModelBase
{
    private IUnitOfWork _unitOfWork;

    [Inject]
    public TeacherViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
