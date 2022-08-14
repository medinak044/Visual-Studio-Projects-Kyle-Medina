using Oversee.Data;
using Oversee.Models;
using Oversee.Repository.IRepository;

namespace Oversee.Repository;

public class UserConnectionRequestRepository : GenericRepository<UserConnectionRequest>, IUserConnectionRequestRepository
{
    private readonly AppDbContext _context;

    public UserConnectionRequestRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
