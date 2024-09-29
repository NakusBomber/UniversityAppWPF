using System.Windows.Input;
using UniversityApp.ViewModel.Commands;

namespace UniversityApp.ViewModel.ViewModels.Dialogs;

public class TeacherDialogViewModel : ViewModelBase
{
    private readonly Action _closeAstion;

    public bool IsSuccess { get; set; }

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

    private string _firstName;
    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged();
        }
    }

    private string _lastName;
    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            OnPropertyChanged();
        }
    }

    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    public TeacherDialogViewModel(string titleWindow, Action closeAction)
    {
        _titleWindow = titleWindow;
        _closeAstion = closeAction;

        _firstName = string.Empty;
        _lastName = string.Empty;

        OkCommand = new RelayCommand(OkClose);
        CancelCommand = new RelayCommand(CancelClose);
    }

    private void CancelClose(object? obj)
    {
        IsSuccess = false;
        _closeAstion?.Invoke();
    }

    private void OkClose(object? obj)
    {
        IsSuccess = true;
        _closeAstion?.Invoke();
    }
}
