using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() // Gets multiple users
        { return await _context.Users.ToListAsync(); }

        // api/users/:id
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id) // Gets one user
        { return await _context.Users.FindAsync(id); }
    }
}
