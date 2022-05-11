using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DatingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        { _context = context; }

        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> GetUsers() // Gets multiple users
        { return _context.Users.ToList(); }

        // datingapp/users/:id
        [HttpGet("{id}")]
        public ActionResult<AppUser> GetUser() // Gets one user
        { return _context.Users.Find(); }
    }
}
