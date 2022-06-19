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
    private readonly IUnitOfWork _unitOfWork;

    public WeaponTypeController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{weaponTypeId}")]
    public async Task<ActionResult<WeaponType>> GetWeaponType(int weaponTypeId)
    {
        var weaponType = await _unitOfWork.WeaponType.GetFirstOrDefault(weaponTypeId);

        if (weaponType == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(weaponType);
    }


    [HttpGet("get-weaponTypes")]
    public async Task<ActionResult<ICollection<WeaponType>>> GetWeaponTypes()
    {
        var weaponTypes = await _unitOfWork.WeaponType.GetAll();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(weaponTypes);
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterWeaponType(WeaponType weaponType)
    {
        if (weaponType == null)
            return BadRequest(ModelState);

        if (await _unitOfWork.WeaponType.Exists(weaponType.Type))
            return BadRequest("Username is taken");

        // Map DTO values to Model
        //WeaponType hero = _mapper.Map<WeaponType>(weaponType);

        _unitOfWork.WeaponType.Add(weaponType); // Get data ready to be saved
        if (!await _unitOfWork.Save()) // Attempt to save, then check if save was successful
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpDelete("delete-weaponType")]
    public async Task<ActionResult> DeleteWeaponType(int weaponType)
    {
        var weaponTypeToDelete = await _unitOfWork.WeaponType.GetFirstOrDefault(weaponType);

        if (weaponTypeToDelete == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _unitOfWork.WeaponType.Remove(weaponTypeToDelete);
        if (!await _unitOfWork.Save())
        {
            ModelState.AddModelError("", "Something went wrong while deleting");
        }

        return NoContent();
    }
}
