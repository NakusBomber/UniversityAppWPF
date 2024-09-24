using System.Linq.Expressions;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Interfaces;

namespace UniversityApp.Model.Tests;

public class FakeRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    private HashSet<TEntity> _set;
    public void Create(TEntity entity)
    {
        _set.Add(entity);
    }

    public async Task CreateAsync(TEntity entity)
    {
        await Task.Run(() => Create(entity));
    }

    public void Delete(TEntity entity)
    {
        _set.Remove(entity);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await Task.Run(() => Delete(entity));
    }

    public IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null, 
        Func<IQueryable<TEntity>, IOrderedEnumerable<TEntity>>? orderBy = null,
        bool asNoTracking = false)
    {
        IQueryable<TEntity> hashSet = (IQueryable<TEntity>)_set;

        if (filter != null)
        {
            hashSet = hashSet.Where(filter);
        }

        if (orderBy != null)
        {
            return orderBy(hashSet).ToList();
        }

        return hashSet.ToList();
    }

    public async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null, 
        Func<IQueryable<TEntity>, IOrderedEnumerable<TEntity>>? orderBy = null,
        bool asNoTracking = false)
    {
        return await Task.Run(() => GetAsync(filter, orderBy, asNoTracking));
    }

    public void Update(TEntity entity)
    {
        if (_set.Remove(entity))
        {
            _set.Add(entity);
        }
        else
        {
            throw new ArgumentException("Entity not found");
        }
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await Task.Run(() => Update(entity));
    }

    public TEntity GetById(Guid id)
    {
        var entity = _set.FirstOrDefault(e => e.Id == id);
        if (entity == null)
        {
            throw new InvalidOperationException("Not found entity by this Id");
        }
        return entity;
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await Task.Run(() => GetById(id));
    }

    public FakeRepository() : this(new HashSet<TEntity>())
    {
    }

    public FakeRepository(HashSet<TEntity> entities)
    {
        _set = entities;
    }
}
