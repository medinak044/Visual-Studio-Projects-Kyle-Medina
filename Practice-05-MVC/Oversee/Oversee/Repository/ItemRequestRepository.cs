using Oversee.Data;
using Oversee.Models;
using Oversee.Repository.IRepository;

namespace Oversee.Repository;

public class ItemRequestRepository : GenericRepository<ItemRequest>, IItemRequestRepository
{
    private readonly AppDbContext _context;

    public ItemRequestRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
