using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;

namespace UniversityApp.Model.Interfaces;

public interface IExporter<TEntity> where TEntity : Entity
{
    public string? FilePath { get; set; }
    public bool IsNeedHeaderline { get; set; }
    public EExportTypes ExportType { get; set; }
    public void Export(IEnumerable<TEntity> exportData);
    public Task ExportAsync(IEnumerable<TEntity> exportData);
}
