using Microsoft.AspNetCore.Mvc;
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
    private readonly IUnitOfWork _unitOfWork;

    public HeroController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{heroId}", Name = "get-hero")]
    public async Task<ActionResult<HeroDto>> GetHero(int heroId)
    {
        var hero = _mapper.Map<HeroDto>(await _unitOfWork.Heroes.GetById(heroId));

        if (hero == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(hero);
    }


    [HttpGet("get-heroes")]
    public async Task<ActionResult<IEnumerable<HeroDto>>> GetHeroes()
    {
        var heroes = await _unitOfWork.Heroes.GetAll();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(heroes);
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterHero(HeroDto heroDto)
    {
        // Check if dto arrives with data
        if (heroDto == null)
            return BadRequest(ModelState);

        // Search the db if the new Hero's name already exists
        if (await _unitOfWork.Heroes.Exists(h =>
        h.UserName.Trim().ToUpper() == heroDto.UserName.Trim().ToUpper()))
            return BadRequest("Username is taken");

        // Map DTO values to Model
        Hero hero = _mapper.Map<Hero>(heroDto);

        // Save data to db + Check if automapping was successful
        await _unitOfWork.Heroes.Add(hero); // "await" executes async method then outputs a bool instead of Task<bool>
        if (!await _unitOfWork.Save())
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        // End method by indicating hero creation was successful
        return Ok("Successfully created");
    }

    [HttpPut("update-hero")]
    public async Task<ActionResult> UpdateHero(HeroDto updatedHero)
    {
        #region Validations
        if (updatedHero == null)
            return BadRequest(ModelState);

        // Check if weaponType to be updated actually exists
        if (!await _unitOfWork.Heroes.Exists(h => h.Id == updatedHero.Id))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();
        #endregion

        var mappedHero = _mapper.Map<Hero>(updatedHero);

        await _unitOfWork.Heroes.Update(mappedHero);
        if (!await _unitOfWork.Save())
        {
            ModelState.AddModelError("", "Something went wrong while updating");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    //[HttpPut("update-hero")]
    //public async Task<ActionResult> UpdateHero(int heroId, HeroDto updatedHero)
    //{
    //    #region Validations
    //    if (updatedHero == null)
    //        return BadRequest(ModelState);

    //    // Check if ids match
    //    if (heroId != updatedHero.Id)
    //        return BadRequest(ModelState);

    //    // Check if weaponType to be updated actually exists
    //    if (!await _unitOfWork.Heroes.Exists(h => h.Id == heroId))
    //        return NotFound();

    //    if (!ModelState.IsValid)
    //        return BadRequest();
    //    #endregion

    //    var mappedHero = _mapper.Map<Hero>(updatedHero);

    //    await _unitOfWork.Heroes.Update(mappedHero);
    //    if (!await _unitOfWork.Save())
    //    {
    //        ModelState.AddModelError("", "Something went wrong while updating");
    //        return StatusCode(500, ModelState);
    //    }

    //    return NoContent();
    //}

    [HttpDelete("delete-hero")]
    public async Task<ActionResult> DeleteHero(int heroId)
    {
        var heroToDelete = await _unitOfWork.Heroes.GetById(heroId);
        //var itemToDelete = await _itemRepository.GetItem(heroId); // Get specific item of the Hero that own's it

        if (heroToDelete == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Must delete Items associated with the Hero
        //if (!await _heroRepository.DeleteItems(itemToDelete))
        //{
        //    ModelState.AddModelError("", "Something went wrong while deleting");
        //}


        // Attempt to delete hero
        await _unitOfWork.Heroes.Remove(heroToDelete);
        if (!await _unitOfWork.Save())
        {
            ModelState.AddModelError("", "Something went wrong while deleting");
        }

        return NoContent();
    }
}
