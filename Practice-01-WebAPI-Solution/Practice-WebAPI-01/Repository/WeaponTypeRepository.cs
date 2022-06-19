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

    public async Task<WeaponType> GetFirstOrDefault(int weaponTypeId)
    {
        return await _context.WeaponTypes.FirstOrDefaultAsync(w => w.Id == weaponTypeId);
    }

    //NOTE: GetAll() could be expanded to set a limit for the amount of records being retrieved.(ex. 1000 returns top 1000, -1 returns everything)
    public async Task<ICollection<WeaponType>> GetAll()
    {
        return await _context.WeaponTypes.ToListAsync();
    }
    public async Task<bool> Exists(string weaponTypeName)
    {
        var x = await _context.WeaponTypes.Where(w => w.Type.Trim().ToUpper() == weaponTypeName.Trim().ToUpper()).FirstOrDefaultAsync();

        if (x != null)
            return true;

        return false;
    }

    public void Add(WeaponType weaponType)
    {
        _context.Add(weaponType);
        //return await Save();
    }
    public void Remove(WeaponType weaponType)
    {
        _context.Remove(weaponType);
        //return await Save();
    }

    //public async Task<bool> Save()
    //{
    //    var saved = await _context.SaveChangesAsync();
    //    return saved > 0 ? true : false;
    //}
}
