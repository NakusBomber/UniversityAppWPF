using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;

namespace UniversityApp.Model.Interfaces;

public interface IExporter
{
    public bool IsNeedHeaderline { get; set; }
    public EExportTypes ExportType { get; set; }
    public void Export(IEnumerable<Student> students);
    public Task ExportAsync(IEnumerable<Student> students);
    public void SetPath(string path);
}
