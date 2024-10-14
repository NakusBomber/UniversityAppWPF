using System.Linq.Expressions;
using UniversityApp.Model.Entities;

namespace UniversityApp.Model.Interfaces;

public interface ICRUD<T>
{
    public IEnumerable<T> Get(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedEnumerable<T>>? orderBy = null,
        bool asNoTracking = false);
    public T GetById(Guid id);
    public void Create(T entity);
    public void UpdateRange(IEnumerable<T> entities);
    public void Update(T entity);
    public void Delete(T entity);
}
