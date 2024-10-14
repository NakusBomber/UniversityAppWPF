using UniversityApp.Model.Entities;

namespace UniversityApp.Model.Interfaces;

public interface IRepository<TEntity> : ICRUD<TEntity>, ICRUDAsync<TEntity>
{
}