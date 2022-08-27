using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Oversee.Models;
using Oversee.Repository.IRepository;

namespace Oversee.Data;

public class DbInitializer : IDbInitializer
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(
        AppDbContext context, // Grants access to helper methods that UnitOfWork does not have
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async void Initialize()
    {
        // Apply migrations if they are not applied
        try
        {
            if (_context.Database.GetPendingMigrations().Count() > 0)
            {
                _context.Database.Migrate();
            }
        }
        catch (Exception ex) { }

        // Create roles if they are not created
        if (!await _roleManager.RoleExistsAsync(AccountRoles_SD.Admin))
        {
            await _roleManager.CreateAsync(new IdentityRole(AccountRoles_SD.Admin));
            await _roleManager.CreateAsync(new IdentityRole(AccountRoles_SD.AppUser));
            await _roleManager.CreateAsync(new IdentityRole(AccountRoles_SD.Example));
        }

        string demoPassword = "Password!23";

        // Create demo admin
        if (await _userManager.FindByEmailAsync("admin@example.com") == null)
        {
            AppUser createdAdminUser = new AppUser
            {
                UserName = "Admin_UserName",
                Email = "admin@example.com",
                FirstName = "Ad",
                LastName = "Min",
            };

            await _userManager.CreateAsync(createdAdminUser, demoPassword);

            AppUser adminUser = await _userManager.FindByEmailAsync(createdAdminUser.Email);
            await _userManager.AddToRoleAsync(adminUser, AccountRoles_SD.AppUser);
            await _userManager.AddToRoleAsync(adminUser, AccountRoles_SD.Admin);
        }

        // Create demo user
        if (await _userManager.FindByEmailAsync("appuser@example.com") == null)
        {
            AppUser createdAdminUser = new AppUser
            {
                UserName = "AppUser_UserName",
                Email = "appuser@example.com",
                FirstName = "App",
                LastName = "User",
            };

            await _userManager.CreateAsync(createdAdminUser, demoPassword);

            AppUser user = await _userManager.FindByEmailAsync(createdAdminUser.Email);
            await _userManager.AddToRoleAsync(user, AccountRoles_SD.AppUser);

            return;
        }
    }
}
