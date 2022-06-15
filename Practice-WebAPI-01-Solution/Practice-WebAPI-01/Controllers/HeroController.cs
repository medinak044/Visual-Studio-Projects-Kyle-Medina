using Microsoft.AspNetCore.Mvc;
using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Models;
using Practice_WebAPI_01.Interfaces;
using AutoMapper;
using Practice_WebAPI_01.DTOs;

namespace Practice_WebAPI_01.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HeroController : ControllerBase // Inherit from ControllerBase instead of Controller because Controller just adds support for views (MVC); not applicable to webAPI
{
    private readonly IMapper _mapper;
    private readonly IHeroRepository _heroRepository;

    public HeroController(IMapper mapper, IHeroRepository heroRepository)
    {
        _mapper = mapper;
        _heroRepository = heroRepository;
    }

    [HttpGet("{heroId}")]
    public async Task<ActionResult<Hero>> GetHero(int heroId)
    {
        var hero = await _heroRepository.GetHero(heroId);

        if (hero == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(hero);
    }


    [HttpGet]
    public async Task<ActionResult<ICollection<Hero>>> GetHeroes()
    {
        var heroes = await _heroRepository.GetHeroes();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(heroes);
    }

    [HttpPost]
    public async Task<ActionResult> RegisterHero(HeroDto heroDto)
    {
        // Check if dto arrives with data
        if (heroDto == null)
            return BadRequest(ModelState);

        // Search the db if the new Hero's name already exists
        if (await _heroRepository.HeroExists(heroDto.UserName))
            return BadRequest("Username is taken");

        // Map DTO values to Model
        var hero = _mapper.Map<Hero>(heroDto);

        // Save data to db + Check if automapping was successful

        // End method by indicating hero creation was successful
        return Ok("Successfully created");
    }


}
