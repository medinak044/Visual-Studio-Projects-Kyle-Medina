using API.Data;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuperHeroController : ControllerBase
{
    private readonly DataContext _context;

    public SuperHeroController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SuperHero>>> GetSuperHeroes()
    {
        return Ok(await _context.SuperHeroes.ToListAsync());
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<SuperHero>>> CreateSuperHero(SuperHero superHero)
    {
        await _context.SuperHeroes.AddAsync(superHero);
        await _context.SaveChangesAsync();
        return Ok(await _context.SuperHeroes.ToListAsync());
    }

    [HttpPut]
    public async Task<ActionResult<IEnumerable<SuperHero>>> UpdateSuperHero(SuperHero superHero)
    {
        var dbSuperHero = await _context.SuperHeroes.FindAsync(superHero.Id);
        if (dbSuperHero == null)
            return NotFound("Hero not found");

        dbSuperHero.Name = superHero.Name;
        dbSuperHero.FirstName = superHero.FirstName;
        dbSuperHero.LastName = superHero.LastName;
        dbSuperHero.Place = superHero.Place;

        await _context.SaveChangesAsync();

        return Ok(await _context.SuperHeroes.ToListAsync());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<IEnumerable<SuperHero>>> DeleteSuperHero(int id)
    {
        var dbHero = await _context.SuperHeroes.FindAsync(id);
        if (dbHero == null)
            return NotFound("Hero not found");

        _context.SuperHeroes.Remove(dbHero);
        await _context.SaveChangesAsync();

        return Ok(await _context.SuperHeroes.ToListAsync());
    }

}
