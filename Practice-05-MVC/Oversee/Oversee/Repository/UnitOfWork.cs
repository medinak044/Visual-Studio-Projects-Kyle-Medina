using Oversee.Data;
using Oversee.Repository.IRepository;

namespace Oversee.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(
        AppDbContext context
        )
    {
        _context = context;
    }

    #region Repository instances
    // AppUser respository already handled by UserManager (because IdentityUser was inherited)
    //public IItemRepository Items => new ItemRepository(_context);
    #endregion

    public async Task<bool> SaveAsync()
    {
        var saved = await _context.SaveChangesAsync(); // Returns a number
        return saved > 0 ? true : false;
    }
    public void Dispose()
    {
        _context.Dispose();
    }

}
