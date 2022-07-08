using System.Net;
using FormulaOneApp.Data;
using FormulaOneApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormulaOneApp.Controllers;

[Route("api/[controller]")] // api/teams
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "AppUser")]
public class TeamsController : ControllerBase
{
    private static AppDbContext _context;
    
    public TeamsController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("GetTeams")]
    public async Task<ActionResult> GetTeams()
    {
        var teams = await _context.Teams.ToListAsync();
        return Ok(teams);
    }

    [HttpGet("GetTeamById/{id:int}")]
    public async Task<ActionResult> GetTeamById(int id)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);

        if (team == null)
            return BadRequest("Invalid Id");

        return Ok(team);
    }

    [HttpPost("CreateTeam")]
    //[Authorize(Policy = "DepartmentPolicy")]
    public async Task<ActionResult> CreateTeam(Team team)
    {
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction("Get", team.Id, team);
    }

    [HttpPatch("UpdateTeam")]
    public async Task<ActionResult> UpdateTeam(int id, string country)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);

        if (team == null)
            return BadRequest("Invalid id");

        team.Country = country;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("DeleteTeam")]
    public async Task<ActionResult> DeleteTeam(int id)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);

        if (team == null)
            return BadRequest("Invalid id");

        _context.Teams.Remove(team);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}