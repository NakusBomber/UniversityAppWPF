using System.Collections.ObjectModel;
using System.Windows.Input;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Interfaces;
using UniversityApp.ViewModel.Validations;

namespace UniversityApp.ViewModel.ViewModels.Dialogs;

public class StudentDialogViewModel : BasicDialogViewModel
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Action _closeAction;

    public bool IsSuccess { get; set; }

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


    private Group? _group;

    public Group? Group
    {
        get => _group;
        set
        {
            _group = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Group> _groups = new();

    public ObservableCollection<Group> Groups
    {
        get => _groups;
        set
        {
            _groups = value;
            OnPropertyChanged();
        }
    }

    public IAsyncCommand<object?> LoadAllDataCommand { get; }
    public ICommand ClearGroupCommand { get; }
    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    public StudentDialogViewModel(
        string titleWindow,
        Action closeAction,
        IUnitOfWork unitOfWork)
        : base(titleWindow)
    {
        _unitOfWork = unitOfWork;
        _closeAction = closeAction;

        LoadAllDataCommand = AsyncCommand.Create(LoadAllDataAsync);

        ClearGroupCommand = new RelayCommand(o => Group = null, o => Group != null);
        OkCommand = new RelayCommand(OkClose, CanOk);
        CancelCommand = new RelayCommand(CancelClose);
        IsSuccess = false;
        _firstName = string.Empty;
        _lastName = string.Empty;

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

    private void OkClose(object? parameter)
    {
        IsSuccess = true;
        _closeAction?.Invoke();
    }

    private void CancelClose(object? parameter)
    {
        IsSuccess = false;
        _closeAction?.Invoke();
    }

    private async Task LoadGroupsAsync(CancellationToken cancellationToken = default)
    {
        Groups = new ObservableCollection<Group>();
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        Groups = new ObservableCollection<Group>(await _unitOfWork.GroupRepository.GetAsync());
    }

    private async Task LoadAllDataAsync(CancellationToken cancellationToken = default)
    {
        await LoadGroupsAsync(cancellationToken);
    }
}
