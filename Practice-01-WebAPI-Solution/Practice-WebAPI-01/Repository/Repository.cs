using Microsoft.EntityFrameworkCore;
using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Interfaces;
using System.Linq.Expressions;

namespace Practice_WebAPI_01.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DataContext _context;
    internal DbSet<T> _dbSet;

    public Repository(DataContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<bool> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        return true;
    }

    public virtual async Task<bool> Exists(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> GetById(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<bool> Remove(T entity)
    {
        _dbSet.Remove(entity);
        return true;
    }

    public virtual async Task<bool> RemoveRange(IEnumerable<T> entity)
    {
        _dbSet.RemoveRange(entity);
        return true;
    }

    public virtual async Task<bool> Update(T entity)
    {
        _context.Update(entity);
        return true; // Remember to call Save() after this
    }

    //// Might have to code a custom Upsert method for each repository
    //public virtual async Task<bool> Upsert(T entity, Expression<Func<T, bool>> predicate)
    //{
    //    var existingEntity = await dbSet.Where(predicate).FirstOrDefaultAsync();

    //    if (existingEntity == null)
    //        return await Add(entity);

    //    // Somehow map entity's values to existingEntity's fields

    //    return true;
    //}
}
