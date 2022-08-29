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
    public IItemRepository Items => new ItemRepository(_context);
    public IItemRecordRepository ItemRecords => new ItemRecordRepository(_context);
    public IItemRecordUserRepository ItemRecordUsers =>  new ItemRecordUserRepository(_context);
    public IItemRequestRepository ItemRequests =>  new ItemRequestRepository(_context);
    public IUserConnectionRequestRepository UserConnectionRequests =>  new UserConnectionRequestRepository(_context);
    public IUserConnectionRequest_UserRepository UserConnectionRequest_Users =>  new UserConnectionRequest_UserRepository(_context);

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
