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
    private readonly IUnitOfWork _unitOfWork;

    public WeaponController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{weaponId}")]
    public async Task<ActionResult<WeaponType>> GetWeapon(int weaponId)
    {
        var weapon = await _unitOfWork.Weapons.GetById(weaponId);

        if (weapon == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(weapon);
    }


    [HttpGet("get-weapons")]
    public async Task<ActionResult<ICollection<WeaponType>>> GetWeapons()
    {
        var weapon = await _unitOfWork.Weapons.GetAll();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(weapon);
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterWeaponType(Weapon weapon)
    {
        if (weapon == null)
            return BadRequest(ModelState);

        if (await _unitOfWork.Weapons.Exists(w => 
        w.Name.Trim().ToUpper() == weapon.Name.Trim().ToUpper()))
            return BadRequest("Username is taken");

        // Map DTO values to Model
        //WeaponType hero = _mapper.Map<WeaponType>(weaponType);

        // Save data to db + Check if automapping was successful
        await _unitOfWork.Weapons.Add(weapon);
        if (!await _unitOfWork.Save())
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpDelete("delete-weapon")]
    public async Task<ActionResult> DeleteWeapon(int weaponId)
    {
        var weaponToDelete = await _unitOfWork.Weapons.GetById(weaponId);

        if (weaponToDelete == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _unitOfWork.Weapons.Remove(weaponToDelete);
        if (!await _unitOfWork.Save())
        {
            ModelState.AddModelError("", "Something went wrong while deleting");
        }

        return NoContent();
    }

}
