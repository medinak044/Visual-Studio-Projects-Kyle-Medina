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

    [HttpGet("GetAllRoles")]
    public async Task<ActionResult> GetAllRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return Ok(roles);
    }

    [HttpPost("CreateRole")]
    public async Task<ActionResult> CreateRole(string roleName)
    {
        // Check if role exists
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (roleExists)
            return BadRequest(new { error = "Role already exists" });

        // Check if role has been added to db successfully
        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        if (!result.Succeeded)
        {
            _logger.LogInformation($"The role {roleName} has not been added");
            return BadRequest(new {error = $"The role {roleName} has not been added" });
        }

        _logger.LogInformation($"The role {roleName} has been added successfully");
        return Ok(new { result = $"The role {roleName} has been added successfully" });
    }

    [HttpGet("GetAllUsers")]
    public async Task<ActionResult> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        return Ok(users);
    }

    [HttpPost("AddUserToRole")]
    public async Task<ActionResult> AddUserToRole(string email, string roleName)
    {
        // Check if the user exists
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            _logger.LogInformation($"The user with the {email} does not exist");
            return BadRequest(new { error = $"User does not exist" });
        }

        // Check if the role exists
        bool roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            _logger.LogInformation($"The role {roleName} does not exist");
            return BadRequest(new { error = $"Role does not exist" });
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);

        // Check if the user has been assigned to the role successfully
        if (!result.Succeeded)
        {
            _logger.LogInformation($"The user was not able to be added to the role");
            return BadRequest(new { error = $"The user was not able to be added to the role" });
        }

        _logger.LogInformation($"User successfully added to the role");
        return Ok(new { result = $"User successfully added to the role" });
    }
}
