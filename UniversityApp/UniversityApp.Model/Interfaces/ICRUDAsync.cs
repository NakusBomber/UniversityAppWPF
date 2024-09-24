using System.Linq.Expressions;
using UniversityApp.Model.Entities;

namespace UniversityApp.Model.Interfaces;

public interface ICRUDAsync<T>
{
    public Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedEnumerable<T>>? orderBy = null,
        bool asNoTracking = false);
    public Task<T> GetByIdAsync(Guid id);
    public Task CreateAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task DeleteAsync(T entity);
}
