using System.Windows.Input;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;

namespace UniversityApp.ViewModel.ViewModels.Dialogs;

public class CreateCourseDialogViewModel : ViewModelBase
{
	private readonly Action closeAction;

	private string _name;
	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			OnPropertyChanged();
		}
	}

	private string _description;
	public string Description
	{
		get => _description;
		set
		{
			_description = value;
			OnPropertyChanged();
		}
	}

	public bool IsSuccess { get; private set; }
	public ICommand OkCommand { get; }
	public ICommand CancelCommand { get; }

	public CreateCourseDialogViewModel(Action closeAction)
	{
		this.closeAction = closeAction;
		_name = string.Empty;
		_description = string.Empty;

		IsSuccess = false;
		OkCommand = new RelayCommand(OkClose);
		CancelCommand = new RelayCommand(CancelClose);
	}

	private void OkClose(object? parameter)
	{
		IsSuccess = true;
		closeAction?.Invoke();
	}

	private void CancelClose(object? parameter)
	{
		IsSuccess = false;
		closeAction?.Invoke();
	}
}
