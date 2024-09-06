using Ninject;
using System.Windows.Input;
using System.Windows.Threading;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.ViewModels.Controls;

namespace UniversityApp.ViewModel.ViewModels.Pages;

public class ShowViewModel : ViewModelBase
{
    private IUnitOfWork _unitOfWork;
    private Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
    public TreeViewModel TreeViewModel { get; set; }

    [Inject]
    public ShowViewModel(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;

        TreeViewModel = new TreeViewModel(unitOfWork);
        if (TreeViewModel.ReloadCoursesCommand.CanExecute(this))
        {
            Task.Run(async () =>
            {
                await TreeViewModel.ReloadCoursesCommand.ExecuteAsync(this);
                _dispatcher.Invoke(() =>
                {
                    CommandManager.InvalidateRequerySuggested();
                });
            });
        }
	}
}
