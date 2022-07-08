using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FormulaOneApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsSetupController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<ClaimsSetupController> _logger;

        public ClaimsSetupController(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            ILogger<ClaimsSetupController> logger
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet("GetAllUserClaims")]
        public async Task<ActionResult> GetAllUserClaims(string email)
        {
            // Check if the user exists
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The user with the {email} does not exist");
                return BadRequest(new { error = $"User does not exist" });
            }

            var userClaims = await _userManager.GetClaimsAsync(user);

            return Ok(userClaims);
        }

        [HttpPost("AddClaimsToUser")]
        public async Task<ActionResult> AddClaimsToUser(string email, string claimName, string claimValue)
        {
            // Check if the user exists
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The user with the {email} does not exist");
                return BadRequest(new { error = $"User does not exist" });
            }

            // Create new claim
            var userClaim = new Claim(claimName, claimValue);

            // Check if claim was successfully added
            var result = await _userManager.AddClaimAsync(user, userClaim);
            if (!result.Succeeded)
            {
                _logger.LogInformation($"Unable to add claim to the user {user}");
                return BadRequest(new { error = $"Unable to add claim to the user {user}" });
            }

            return Ok(new
            {
                result = $"User {user.Email} has a claim {claimName} added to them"
            });
        }
    }
}
