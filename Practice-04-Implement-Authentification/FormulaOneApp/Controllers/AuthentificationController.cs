﻿using Microsoft.AspNetCore.Http;
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
using Microsoft.EntityFrameworkCore;

namespace FormulaOneApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthentificationController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AuthentificationController> _logger;
    private readonly TokenValidationParameters _tokenValidationParams;

    public AuthentificationController(
        AppDbContext context,
        UserManager<IdentityUser> userManager,
        IConfiguration configuration,
        RoleManager<IdentityRole> roleManager,
        ILogger<AuthentificationController> logger,
        TokenValidationParameters tokenValidationParams
        )
    {
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
        _roleManager = roleManager;
        _logger = logger;
        _tokenValidationParams = tokenValidationParams;
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
                Success = false,
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
                Success = false,
                Errors = new List<string>() { "Server error" }
            });
        }

        // Add user to a default role
        await _userManager.AddToRoleAsync(newUser, RolesString.RoleTypeEnum.AppUser.ToString());

        // Generate the token
        var jwtToken = await GenerateJwtTokenAsync_1(newUser);

        return Ok(jwtToken); // Return the token
    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] UserLoginRequestDto loginRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(new AuthResult()
            {
                Success = false,
                Errors = new List<string>() { "Invalid payload" }
            });

        // Check if user exists
        var existingUser = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (existingUser == null)
        {
            return BadRequest(new AuthResult()
            {
                Success = false,
                Errors = new List<string>() { "Email doesn't exist" }
            });
        }

        // Verify password
        var isCorrect = await _userManager.CheckPasswordAsync(existingUser, loginRequest.Password);
        if (!isCorrect)
        {
            return BadRequest(new AuthResult()
            {
                Success = false,
                Errors = new List<string>() { "Invalid credentials" }
            });
        }

        var jwtToken = await GenerateJwtTokenAsync_1(existingUser);

        return Ok(jwtToken);
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResult()
            {
                Success = false,
                Errors = new List<string>() { "Invalid payload" }
            });
        }

        var result = await VerifyAndGenerateToken(tokenRequest);
        if (result == null) // In case something goes wrong in the middle of the process
        {
            return BadRequest(new AuthResult()
            {
                Success = false,
                Errors = new List<string>() { "Invalid token" }
            });
        }
        return Ok(result);
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
    private async Task<AuthResult> GenerateJwtTokenAsync_1(IdentityUser user)
    {

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value));
        var currentDate = DateTime.UtcNow; // https://stackoverflow.com/questions/64256500/handler-createjwtsecuritytokendescriptor-idx12401
        var claims = await GetAllValidClaimsAsync_2(user);

        // Token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = currentDate.AddSeconds(30), // Temp: For refresh token demo purposes
            //Expires = currentDate.AddHours(1),
            NotBefore = currentDate,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256) // Use .HmacSha512Signature instead
        };

        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            IsUsed = false,
            IsRevoked = false,
            UserId = user.Id,
            AddedDate = currentDate,
            ExpiryDate = currentDate.AddMonths(6),
            Token = RandomString(35) + Guid.NewGuid(),
        };

        await _context.RefreshTokens.AddAsync(refreshToken); // Add changes to memory
        await _context.SaveChangesAsync(); // Save changes to db

        return new AuthResult()
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            Success = true,
        };
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
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // For "JwtId"
            // new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
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

    private async Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        try // Run the token request through validations
        {
            // Check that the string is actually in jwt token format
            // (The token validation parameters were defined in the Program.cs class)
            var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token,
                _tokenValidationParams, out var validatedToken);

            // Check if the encryption algorithm matches
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                bool result = jwtSecurityToken.Header.Alg
                    .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture);

                if (result == false)
                    return null;
            }

            // Check if token has expired (don't generate new token if current token is still usable)
            // "long" was used because of the long utc time string
            var utcExpiryDate = long.Parse(tokenInVerification.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            // Convert into a usable date type
            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

            if (expiryDate > DateTime.UtcNow)
            {
                return new AuthResult() { 
                    Success = false,
                    Errors = new List<string>() { "Token has not yet expired" }
                };
            }

            // Check if token exists in db
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedToken == null)
            {
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>() { "Token does not exist" }
                };
            }

            // Check if token is already used
            if (storedToken.IsUsed)
            {
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>() { "Token has been used" }
                };
            }

            // Check if token has been revoked
            if (storedToken.IsRevoked)
            {
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>() { "Token has been revoked" }
                };
            }

            // Check if jti matches the id of the refresh token that exists in our db (validate the id)
            var jti = tokenInVerification.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (storedToken.JwtId != jti)
            {
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>() { "Token does not match" }
                };
            }

            // First, update current token
            storedToken.IsUsed = true; // Prevent the current token from being used in the future
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync(); // Save changes

            // Then, generate a new jwt token, then assign it to the user
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId); // Find the IdentityUser by the user id on the current token
            return await GenerateJwtTokenAsync_1(dbUser);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    // Converts date data to DateTime
    private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
        return dateTimeVal;
    }

    private string RandomString(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(x => x[random.Next(x.Length)]).ToArray());
    }
}
