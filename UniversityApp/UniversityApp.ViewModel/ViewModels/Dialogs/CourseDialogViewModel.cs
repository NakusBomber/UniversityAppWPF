using Azure.Core;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Validations;

namespace UniversityApp.ViewModel.ViewModels.Dialogs;

public class CourseDialogViewModel : ValidationViewModelBase
{
	private readonly Action closeAction;

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


	private string _name;

    public string Name
	{
		get => _name;
		set
		{
			_name = value;
			ValidateName();
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
			ValidateDescription();
			OnPropertyChanged();
		}
	}

	public bool IsSuccess { get; private set; }
	public ICommand OkCommand { get; }
	public ICommand CancelCommand { get; }

	public CourseDialogViewModel(string title, Action closeAction)
	{
		_titleWindow = title;
		this.closeAction = closeAction;
		_name = string.Empty;
		_description = string.Empty;

		IsSuccess = false;
		OkCommand = new RelayCommand(OkClose, CanOk);
		CancelCommand = new RelayCommand(CancelClose);

		ValidateAll();
	}

	private void ValidateAll()
	{
		ValidateName();
		ValidateDescription();
    }
	
	private void ValidateName()
	{
        Validate(_name, new EntityNameValidationRule(1, 75), nameof(Name));
    }
	
	private void ValidateDescription()
	{
        Validate(_description, new LengthValidationRule(0, 300), nameof(Description));
    }
   
	private bool CanOk(object? arg)
    {
		return !HasErrors;
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
