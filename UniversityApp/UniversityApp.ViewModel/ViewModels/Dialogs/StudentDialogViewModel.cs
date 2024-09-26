using System.Collections.ObjectModel;
using System.Windows.Input;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Interfaces;

namespace UniversityApp.ViewModel.ViewModels.Dialogs;

public class StudentDialogViewModel : ViewModelBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Action _closeAction;

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

    private ObservableCollection<Group>? _groups;

    public ObservableCollection<Group> Groups
    {
        get
        {
            if (_groups == null)
            {
                _groups = new ObservableCollection<Group>();
            }
            return _groups;
        }
        set
        {
            _groups = value;
            OnPropertyChanged();
        }
    }

    public IAsyncCommand LoadAllDataCommand { get; }
    public ICommand ClearGroupCommand { get; }
    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    public StudentDialogViewModel(
        string titleWindow,
        Action closeAction,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _closeAction = closeAction;

        LoadAllDataCommand = AsyncCommand.Create(LoadAllDataAsync);

        ClearGroupCommand = new RelayCommand(o => Group = null);
        OkCommand = new RelayCommand(OkClose);
        CancelCommand = new RelayCommand(CancelClose);
        IsSuccess = false;
        _titleWindow = titleWindow;
        _firstName = string.Empty;
        _lastName = string.Empty;
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
