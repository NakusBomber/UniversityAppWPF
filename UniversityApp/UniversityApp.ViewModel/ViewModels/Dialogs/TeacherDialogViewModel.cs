using System.Windows.Input;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Validations;

namespace UniversityApp.ViewModel.ViewModels.Dialogs;

public class TeacherDialogViewModel : ValidationViewModelBase
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
            ValidateFirstName();
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
            ValidateLastName();
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

        OkCommand = new RelayCommand(OkClose, CanOk);
        CancelCommand = new RelayCommand(CancelClose);

        ValidateAll();
    }

    private void ValidateAll()
    {
        ValidateFirstName();
        ValidateLastName();
    }
    private void ValidateFirstName() => Validate(_firstName, new EntityNameValidationRule(1, 50), nameof(FirstName));
    private void ValidateLastName() => Validate(_lastName, new EntityNameValidationRule(1, 50), nameof(LastName));

    private bool CanOk(object? arg)
    {
        return !HasErrors;
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
