using Microsoft.EntityFrameworkCore;
using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Interfaces;
using System.Linq.Expressions;

namespace Practice_WebAPI_01.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DataContext _context;
    internal DbSet<T> dbSet;

    public Repository(DataContext context)
    {
        _context = context;
        this.dbSet = _context.Set<T>();
    }

    public virtual async Task<bool> Add(T entity)
    {
        await dbSet.AddAsync(entity);
        return true;
    }

    //// XXX
    //public virtual async Task<bool> Exists(string name)
    //{
    //    name = name.ToUpper();
    //    //var x = await _context..Where(h => h.UserName.Trim().ToUpper() == name.Trim().ToUpper()).FirstOrDefaultAsync();
        
    //    // Make sure record's name and argument's name are .ToUpper() for comparison
    //    var x = await dbSet.

    //    if (x != null)
    //        return true; // If Hero name already exists

    //    return false;
    //}

    public virtual async Task<bool> Exists(Expression<Func<T, bool>> predicate)
    {
        var x = await dbSet.Where(predicate).FirstOrDefaultAsync();

        if (x != null)
            return true; // If already exists

        return false;
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await dbSet.ToListAsync();
    }

    ////XXX
    //public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter)
    //{
    //    IQueryable<T> query = dbSet;

    //    //query = await query.FirstOrDefaultAsync(filter);

    //    //return query.FirstOrDefault();
    //    query = query.Where(filter);

    //    return query.FirstOrDefault();
    //}

    //GetById() replaces GetFirstOrDefault()
    public virtual async Task<T> GetById(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task<bool> Remove(T entity)
    {
        dbSet.Remove(entity);
        return true;
    }

    public virtual async Task<bool> RemoveRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
        return true;
    }
}
