using Microsoft.Win32;
using System.IO;
using System.Windows;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;
using UniversityApp.ViewModel.Commands;
using UniversityApp.ViewModel.Interfaces;

namespace UniversityApp.ViewModel.ViewModels.Dialogs;

public class ExportDialogViewModel : ViewModelBase
{
	private readonly Group _group;
	private readonly IExporter _exporter;

	private bool _isNeedHeaderline;
	public bool IsNeedHeaderline
	{
		get => _isNeedHeaderline;
		set
		{
			_isNeedHeaderline = value;
			OnPropertyChanged();
		}
	}

	public Visibility CheckBoxHeadersVisibility
    {
		get
		{
			if (SelectedExtension == EExportTypes.CSV)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}
	}


	private EExportTypes _selectedExtension;

	public EExportTypes SelectedExtension
	{
		get => _selectedExtension;
		set
		{
			_selectedExtension = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(CheckBoxHeadersVisibility));
		}
	}


	public IEnumerable<EExportTypes> ExportTypes => EExportTypesExtension.AllTypes;
	public IAsyncCommand ExportCommand { get; }


	public ExportDialogViewModel(Group group, IExporter exporter)
	{
		_group = group;
		_exporter = exporter;

		_isNeedHeaderline = false;
		SelectedExtension = EExportTypes.PDF;

		ExportCommand = AsyncCommand.Create(ExportStudentsAsync);
	}

    private async Task ExportStudentsAsync(CancellationToken cancellationToken = default)
    {
		SaveFileDialog saveFileDialog = new SaveFileDialog();
		saveFileDialog.Filter = SelectedExtension.GetFilter();

		var courseName = _group.Course != null ? _group.Course!.Name : "CourseName";
		var groupName = _group.Name;

		saveFileDialog.CheckPathExists = true;
		saveFileDialog.FileName = $"{courseName}_{groupName}_listOfStudents";
		if (saveFileDialog.ShowDialog() == true)
		{
			string path = saveFileDialog.FileName;
            _exporter.IsNeedHeaderline = IsNeedHeaderline;
            _exporter.ExportType = SelectedExtension;
            _exporter.SetPath(path);
            await _exporter.ExportAsync(_group.Students);
        }
    }
}
