using System.Windows.Input;
using UniversityApp.ViewModel.Commands;

namespace UniversityApp.ViewModel.ViewModels.Dialogs;

public class MessageBoxViewModel : BasicDialogViewModel
{
	private readonly Action _closeAction;

	private string _message;
	public string Message
	{
		get => _message;
		set
		{
			_message = value;
			OnPropertyChanged();
		}
	}

	public ICommand CloseCommand { get; }

	public MessageBoxViewModel(string title, string message, Action closeAction)
		: base(title)
	{
		_message = message;
		_closeAction = closeAction;

		CloseCommand = new RelayCommand(Close);
	}

	private void Close(object? parameter)
	{
		_closeAction?.Invoke();
	}

}
