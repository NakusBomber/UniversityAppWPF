using UniversityApp.Model.Entities;

namespace UniversityApp.Model.Interfaces;

public interface IImporter<TResult> where TResult : class
{
    public TResult Import(string path);
    public Task<TResult> ImportAsync(string path);
}
