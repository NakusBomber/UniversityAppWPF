using UniversityApp.ViewModel.ViewModels;
using UniversityApp.ViewModel.ViewModels.Dialogs;

namespace UniversityApp.ViewModel.Interfaces;

public interface IWindowService<TViewModel ,TResult> where TViewModel : BasicDialogViewModel
                                                     where TResult : class
{
    public TResult Show(TViewModel viewModel);
    public Task<TResult> ShowAsync(TViewModel viewModel);
}

public interface IWindowService<TViewModel> where TViewModel : BasicDialogViewModel
{
    public void Show(TViewModel viewModel);
    public Task ShowAsync(TViewModel viewModel);
}
