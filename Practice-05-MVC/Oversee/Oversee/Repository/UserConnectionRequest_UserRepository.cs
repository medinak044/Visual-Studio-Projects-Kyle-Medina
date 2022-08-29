using Oversee.Data;
using Oversee.Models;
using Oversee.Repository.IRepository;

namespace Oversee.Repository;

public class UserConnectionRequest_UserRepository : GenericRepository<UserConnectionRequest_User>, IUserConnectionRequest_UserRepository
{
    private readonly AppDbContext _context;

    public UserConnectionRequest_UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
