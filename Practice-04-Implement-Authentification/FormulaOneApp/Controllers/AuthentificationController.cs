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

        // Generate the token
        var jwtToken = GenerateJwtToken_1(newUser);

        return Ok(new AuthResult()
        {
            Result = true,
            Token = jwtToken
        });
    }

    [Route("Login")]
    [HttpPost]
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

        var jwtToken = GenerateJwtToken_2(existingUser);

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
    private string GenerateJwtToken_1(IdentityUser user)
    {

        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
        var currentDate = DateTime.UtcNow; // https://stackoverflow.com/questions/64256500/handler-createjwtsecuritytokendescriptor-idx12401

        // Token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, value:user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
            }),

            Expires = currentDate.AddHours(1),
            NotBefore = currentDate,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        return jwtTokenHandler.WriteToken(token);
    }

    private string GenerateJwtToken_2(IdentityUser user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
        var currentDate = DateTime.UtcNow; // https://stackoverflow.com/questions/64256500/handler-createjwtsecuritytokendescriptor-idx12401

        var claims = new[]
            {
            new Claim("Id", user.Id), // There is no ClaimTypes.Id
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            //new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
            //new Claim(ClaimTypes.Surname, loggedInUser.Surname),
            //new Claim(ClaimTypes.Role, loggedInUser.Role)
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
}
