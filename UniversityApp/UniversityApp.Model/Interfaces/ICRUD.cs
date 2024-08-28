using System.Linq.Expressions;
using UniversityApp.Model.Entities;

namespace UniversityApp.Model.Interfaces;

public interface ICRUD<T>
{
    public IEnumerable<T> Get(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedEnumerable<T>>? orderBy = null);
    public void Create(T entity);
    public void Update(T entity);
    public void Delete(T entity);
}
