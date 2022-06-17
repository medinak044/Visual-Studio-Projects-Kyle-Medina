using Microsoft.AspNetCore.Mvc;
using Practice_WebAPI_01.Models;
using Practice_WebAPI_01.Interfaces;
using AutoMapper;
using Practice_WebAPI_01.DTOs;

namespace Practice_WebAPI_01.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeaponTypeController: ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IWeaponTypeRepository _weaponTypeRepository;

    public WeaponTypeController(IMapper mapper, IWeaponTypeRepository weaponTypeRepository)
    {
        _mapper = mapper;
        _weaponTypeRepository = weaponTypeRepository;
    }

    [HttpGet("{weaponTypeId}")]
    public async Task<ActionResult<WeaponType>> GetWeaponType(int weaponTypeId)
    {
        var weaponType = await _weaponTypeRepository.GetWeaponType(weaponTypeId);

        if (weaponType == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(weaponType);
    }


    [HttpGet("get-weaponTypes")]
    public async Task<ActionResult<ICollection<WeaponType>>> GetWeaponType()
    {
        var weaponTypes = await _weaponTypeRepository.GetWeaponTypes();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(weaponTypes);
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterWeaponType(WeaponType weaponType)
    {
        if (weaponType == null)
            return BadRequest(ModelState);

        if (await _weaponTypeRepository.WeaponTypeExists(weaponType.Type))
            return BadRequest("Username is taken");

        // Map DTO values to Model
        //WeaponType hero = _mapper.Map<WeaponType>(weaponType);

        // Save data to db + Check if automapping was successful
        var result = await _weaponTypeRepository.RegisterWeaponType(weaponType); // "await" executes async method then outputs a bool instead of Task<bool>
        if (!result)
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpDelete("delete-weaponType")]
    public async Task<ActionResult> DeleteWeaponType(int weaponType)
    {
        var weaponTypeToDelete = await _weaponTypeRepository.GetWeaponType(weaponType);

        if (weaponTypeToDelete == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _weaponTypeRepository.DeleteWeaponType(weaponTypeToDelete))
        {
            ModelState.AddModelError("", "Something went wrong while deleting");
        }

        return NoContent();
    }
}
