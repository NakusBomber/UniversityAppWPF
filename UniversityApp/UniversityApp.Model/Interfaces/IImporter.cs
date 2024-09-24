using UniversityApp.Model.Entities;

namespace UniversityApp.Model.Interfaces;

public interface IImporter<TEntity> where TEntity : Entity
{
    public IEnumerable<TEntity> Import(string path);
    public Task<IEnumerable<TEntity>> ImportAsync(string path);
}
