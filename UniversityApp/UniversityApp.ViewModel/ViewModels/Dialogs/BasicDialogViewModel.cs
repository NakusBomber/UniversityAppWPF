namespace UniversityApp.ViewModel.ViewModels.Dialogs;

public class BasicDialogViewModel : ValidationViewModelBase
{
    private string _titleWindow;
    public string TitleWindow
    {
        get => _titleWindow;
        set
        {
            _titleWindow = value;
            OnPropertyChanged();
        }
    }

    public BasicDialogViewModel(string titleWindow)
    {
        _titleWindow = titleWindow;
    }
}
