using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UniversityApp.Model.Entities;
using UniversityApp.Model.Helpers;
using UniversityApp.Model.Interfaces;

namespace UniversityApp.Model.Repositories;

public class GeneralRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    private ApplicationContext _context;
    private DbSet<TEntity> _entities;

    public GeneralRepository(ApplicationContext context)
    {
        _context = context;
        _entities = _context.Set<TEntity>();
    }

    public void Create(TEntity entity)
    {
        CreateAsync(entity).Wait();
    }
    public IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedEnumerable<TEntity>>? orderBy = null)
    {
        return GetAsync(filter, orderBy).Result;
    }

    public void Update(TEntity entity)
    {
        _entities.Attach(entity);
        _entities.Entry(entity).State = EntityState.Modified;
    }
    public void Delete(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _entities.Attach(entity);
        }
        _entities.Remove(entity);
    }


    public async Task CreateAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedEnumerable<TEntity>>? orderBy = null)
    {
        IQueryable<TEntity> dbSet = _entities;
        
        if (filter != null)
        {
            dbSet = dbSet.Where(filter);
        }

        if (orderBy != null)
        {
            dbSet = (IQueryable<TEntity>)orderBy(dbSet);
        }

        return await dbSet.ToListAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await Task.Run(() => Update(entity));
    }
    public async Task DeleteAsync(TEntity entity)
    {
        await Task.Run(() => Delete(entity));
    }
}
