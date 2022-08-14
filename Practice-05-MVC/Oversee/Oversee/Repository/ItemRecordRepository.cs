using Oversee.Data;
using Oversee.Models;
using Oversee.Repository.IRepository;

namespace Oversee.Repository;

public class ItemRecordRepository : GenericRepository<ItemRecord>, IItemRecordRepository
{
    private readonly AppDbContext _context;

    public ItemRecordRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
