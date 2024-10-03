using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;

namespace UniversityApp.Model.Tests;

public class StudentExporterFake : IExporter<Student>
{
    private string? _filePath;
    public string? FilePath
    {
        get => _filePath;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Path is null or empty");
            }

            _filePath = value;
        }
    }
    public bool IsNeedHeaderline { get ; set ; }
    public EExportTypes ExportType { get; set; }

    public StudentExporterFake()
    {
        IsNeedHeaderline = false;
        ExportType = EExportTypes.CSV;
    }

    public void Export(IEnumerable<Student> exportData)
    {
        if (string.IsNullOrEmpty(FilePath))
        {
            throw new ArgumentException("Path is null or empty");
        }
    }

    public async Task ExportAsync(IEnumerable<Student> exportData)
    {
        await Task.Run(() =>
        {
            Export(exportData);
        });
    }
}
