using Microsoft.EntityFrameworkCore;
using Oversee.Data;
using Oversee.Repository.IRepository;
using System.Linq.Expressions;

namespace Oversee.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context;
    internal DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<bool> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return true;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> GetSome(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public bool Remove(T entity)
    {
        _dbSet.Remove(entity);
        return true;
    }

    public bool RemoveRange(IEnumerable<T> entity)
    {
        _dbSet.RemoveRange(entity);
        return true;
    }

    public bool Update(T entity)
    {
        _context.Update(entity);
        return true; // Remember to save changes to db after this method executes
    }
}
