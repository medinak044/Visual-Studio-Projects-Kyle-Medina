using Practice_WebAPI_01.Interfaces;
using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Models;
using Microsoft.EntityFrameworkCore;

namespace Practice_WebAPI_01.Repository;

public class WeaponTypeRepository: IWeaponTypeRepository
{
    private readonly DataContext _context;

    public WeaponTypeRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<WeaponType> GetWeaponType(int weaponTypeId)
    {
        return await _context.WeaponTypes.FirstOrDefaultAsync(w => w.Id == weaponTypeId);
    }

    public async Task<ICollection<WeaponType>> GetWeaponTypes()
    {
        return await _context.WeaponTypes.ToListAsync();
    }
    public async Task<bool> WeaponTypeExists(string weaponTypeName)
    {
        var x = await _context.WeaponTypes.Where(w => w.Type.Trim().ToUpper() == weaponTypeName.Trim().ToUpper()).FirstOrDefaultAsync();

        if (x != null)
            return true;

        return false;
    }

    public async Task<bool> RegisterWeaponType(WeaponType weaponType)
    {
        await _context.AddAsync(weaponType);
        return await Save();
    }
    public async Task<bool> DeleteWeaponType(WeaponType weaponType)
    {
        _context.Remove(weaponType);
        return await Save();
    }

    public async Task<bool> Save()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }

}
