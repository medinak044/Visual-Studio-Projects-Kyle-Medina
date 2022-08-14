using Oversee.Data;
using Oversee.Models;
using Oversee.Repository.IRepository;

namespace Oversee.Repository;

public class ItemRecordUserRepository : GenericRepository<ItemRecordUser>, IItemRecordUserRepository
{
    private readonly AppDbContext _context;

    public ItemRecordUserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
