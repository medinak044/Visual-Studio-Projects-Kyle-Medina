using Microsoft.AspNetCore.Mvc;
using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Models;
using Practice_WebAPI_01.Interfaces;
using AutoMapper;

namespace Practice_WebAPI_01.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HeroController : ControllerBase // Inherit from ControllerBase instead of Controller because Controller just adds support for views (MVC); not applicable to webAPI
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IHeroRepository _heroRepository;

    public HeroController(DataContext context, IMapper mapper, IHeroRepository heroRepository)
    {
        _context = context;
        _mapper = mapper;
        _heroRepository = heroRepository;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Hero>>> GetHeroes()
    {

    }

}
