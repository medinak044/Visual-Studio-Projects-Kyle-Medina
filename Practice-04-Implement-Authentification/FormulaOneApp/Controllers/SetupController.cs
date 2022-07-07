using FormulaOneApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormulaOneApp.Controllers;

[Route("api/[controller]")] // api/teams
[ApiController]
public class SetupController: ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<SetupController> _logger;

    public SetupController(
        AppDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<SetupController> logger
        )
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    [HttpGet("GetRoles")]
    public async Task<ActionResult> GetAllRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return Ok(roles);
    }
}
