using Microsoft.AspNetCore.Mvc;
using Practice_WebAPI_01.Models;
using Practice_WebAPI_01.Interfaces;
using AutoMapper;
using Practice_WebAPI_01.DTOs;

namespace Practice_WebAPI_01.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeaponController : ControllerBase
{
    private readonly IWeaponRepository _weaponRepository;

    public WeaponController(IWeaponRepository weaponRepository)
    {
        _weaponRepository = weaponRepository;
    }

    [HttpGet("{weaponId}")]
    public async Task<ActionResult<WeaponType>> GetWeapon(int weaponId)
    {
        var weapon = await _weaponRepository.GetWeapon(weaponId);

        if (weapon == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(weapon);
    }


    [HttpGet("get-weapons")]
    public async Task<ActionResult<ICollection<WeaponType>>> GetWeapons()
    {
        var weapon = await _weaponRepository.GetWeapons();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(weapon);
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterWeaponType(Weapon weapon)
    {
        if (weapon == null)
            return BadRequest(ModelState);

        if (await _weaponRepository.WeaponExists(weapon.Name))
            return BadRequest("Username is taken");

        // Map DTO values to Model
        //WeaponType hero = _mapper.Map<WeaponType>(weaponType);

        // Save data to db + Check if automapping was successful
        var result = await _weaponRepository.RegisterWeapon(weapon); // "await" executes async method then outputs a bool instead of Task<bool>
        if (!result)
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpDelete("delete-weapon")]
    public async Task<ActionResult> DeleteWeapon(int weapon)
    {
        var weaponToDelete = await _weaponRepository.GetWeapon(weapon);

        if (weaponToDelete == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _weaponRepository.DeleteWeapon(weaponToDelete))
        {
            ModelState.AddModelError("", "Something went wrong while deleting");
        }

        return NoContent();
    }

}
