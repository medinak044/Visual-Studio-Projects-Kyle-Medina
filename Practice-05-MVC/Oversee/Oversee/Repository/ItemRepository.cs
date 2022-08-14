using Oversee.Data;
using Oversee.Models;
using Oversee.Repository.IRepository;

namespace Oversee.Repository;

public class ItemRepository : GenericRepository<Item>, IItemRepository
{
    private readonly AppDbContext _context;

    public ItemRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
