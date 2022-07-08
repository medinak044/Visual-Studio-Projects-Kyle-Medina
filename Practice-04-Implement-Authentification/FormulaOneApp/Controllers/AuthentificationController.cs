using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FormulaOneApp.Data;
using Microsoft.AspNetCore.Identity;
using FormulaOneApp.Configurations;
using FormulaOneApp.Models.DTOs;
using FormulaOneApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using FormulaOneApp.Helpers;

namespace FormulaOneApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthentificationController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AuthentificationController> _logger;

    public AuthentificationController(
        UserManager<IdentityUser> userManager,
        IConfiguration configuration,
        RoleManager<IdentityRole> roleManager,
        ILogger<AuthentificationController> logger
        )
    {
        _userManager = userManager;
        _configuration = configuration;
        _roleManager = roleManager;
        _logger = logger;
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
    {
        // Validate incoming request
        if (!ModelState.IsValid)
            return BadRequest();
        // Check if email already exists
        var existingUser = await _userManager.FindByEmailAsync(requestDto.Email);
        if (existingUser != null)
        {
            return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>() { "Email already exists" }
            });
        }

        var newUser = new IdentityUser()
        {
            Email = requestDto.Email,
            UserName = requestDto.Email
        };

        // Note: Identity requires passwords to have X amount of characters, capitalized letter, number, and symbol
        var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);
        if (!isCreated.Succeeded)
        {
            return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>() { "Server error" }
            });
        }

        // Add user to a default role
        await _userManager.AddToRoleAsync(newUser, RolesString.RoleTypeEnum.AppUser.ToString());

        // Generate the token
        var jwtToken = await GenerateJwtTokenAsync_1(newUser);

        return Ok(new AuthResult()
        {
            Result = true,
            Token = jwtToken
        });
    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] UserLoginRequestDto loginRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>() { "Invalid payload" }
            });

        // Check if user exists
        var existingUser = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (existingUser == null)
        {
            return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>() { "Email doesn't exist" }
            });
        }

        // Verify password
        var isCorrect = await _userManager.CheckPasswordAsync(existingUser, loginRequest.Password);
        if (!isCorrect)
        {
            return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>() { "Invalid credentials" }
            });
        }

        var jwtToken = await GenerateJwtTokenAsync_1(existingUser);

        return Ok(new AuthResult()
        {
            Result = true,
            Token = jwtToken
        });
    }


    #region GenerateJwtToken notes
    //private string GenerateJwtToken(IdentityUser user)
    //{
    //    var jwtTokenHandler = new JwtSecurityTokenHandler(); // Create token handler

    //    var key = Encoding.UTF8.GetBytes(_configuration.GetSection(key: "JwtConfig:Secret").Value); // Created key from configuration

    //    // Create new token descriptor and fill it with new token info
    //    var tokenDescriptor = new SecurityTokenDescriptor() 
    //    {
    //        Subject = new ClaimsIdentity(new[]
    //        {
    //            new Claim("Id", user.Id),
    //            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
    //            new Claim(JwtRegisteredClaimNames.Email, user.Email),
    //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
    //        }),
    //        Expires = DateTime.Now.AddHours(1), // Define token's lifetime before expiration
    //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256) // https://youtu.be/Y-MjCw6thao?t=4886
    //    };

    //    var token = jwtTokenHandler.CreateToken(tokenDescriptor); // Combine token descriptor with handler
    //    var jwtToken = jwtTokenHandler.WriteToken(token); // Converts from SecurityToken to string

    //    return jwtToken;
    //}
    #endregion
    private async Task<string> GenerateJwtTokenAsync_1(IdentityUser user)
    {

        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
        var currentDate = DateTime.UtcNow; // https://stackoverflow.com/questions/64256500/handler-createjwtsecuritytokendescriptor-idx12401
        var claims = await GetAllValidClaimsAsync_2(user);

        // Token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            //Subject = new ClaimsIdentity(new[]
            //{
            //    new Claim("Id", user.Id),
            //    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            //    new Claim(JwtRegisteredClaimNames.Email, value:user.Email),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
            //}),
            Subject = new ClaimsIdentity(claims),
            Expires = currentDate.AddHours(1),
            NotBefore = currentDate,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        return jwtTokenHandler.WriteToken(token);
    }

    // Source: https://www.youtube.com/watch?v=f2IdQqpjR0c
    private async Task<string> GenerateJwtTokenAsync_2(IdentityUser user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
        var currentDate = DateTime.UtcNow; // https://stackoverflow.com/questions/64256500/handler-createjwtsecuritytokendescriptor-idx12401

        var claims = new[]
            {
            new Claim("Id", user.Id), // There is no ClaimTypes.Id
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            //new Claim(ClaimTypes.Role, user)
            };

        var token = new JwtSecurityToken
        (
            //issuer: builder.Configuration["Jwt:Issuer"],
            //audience: builder.Configuration["Jwt:Audience"],
            claims: claims,
            expires: currentDate.AddHours(1),
            notBefore: currentDate,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }

    // Get all valid claims for the corresponding user. Explanation: https://youtu.be/eVxzuOxWEiY?t=4259
    private async Task<List<Claim>> GetAllValidClaimsAsync_2(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id), // There is no ClaimTypes.Id
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            // (The role claim will be added here)
        };

        // Getting the claims that we have assigned to the user
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        // Get the user role, convert it, and add it to the claims
        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(userRole);

            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var roleClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var roleClaim in roleClaims)
                { claims.Add(roleClaim); }
            }
        }

        return claims;
    }
}
