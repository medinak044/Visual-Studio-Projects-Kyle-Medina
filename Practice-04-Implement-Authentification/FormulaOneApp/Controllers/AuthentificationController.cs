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

namespace FormulaOneApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthentificationController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    //private readonly JwtConfig _jwtConfig;

    public AuthentificationController(
        UserManager<IdentityUser> userManager,
        IConfiguration configuration
        //JwtConfig jwtConfig
        )
    {
        _userManager = userManager;
        _configuration = configuration;
        //_jwtConfig = jwtConfig;
    }

    [HttpPost]
    [Route("Register")]
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

        var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);
        if (!isCreated.Succeeded)
        {
            return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>() { "Server error" }
            });
        }

        // Generate token
        var token = GenerateJwtToken(newUser);

        return Ok(new AuthResult()
        {
            Result = true,
            Token = token
        });
    }


    private string GenerateJwtToken(IdentityUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler(); // Create token handler

        var key = Encoding.UTF8.GetBytes(_configuration.GetSection(key: "JwtConfig:Secret").Value); // Created key from configuration

        // Create new token descriptor and fill it with new token info
        var tokenDescriptor = new SecurityTokenDescriptor() 
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
            }),
            Expires = DateTime.Now.AddHours(1), // Define token's lifetime before expiration
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256) // https://youtu.be/Y-MjCw6thao?t=4886
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor); // Combine token descriptor with handler
        var jwtToken = jwtTokenHandler.WriteToken(token); // Converts from SecurityToken to string

        return jwtToken;
    }



}
