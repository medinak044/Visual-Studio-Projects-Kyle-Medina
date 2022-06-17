using Practice_WebAPI_01.Interfaces;
using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Models;
using Microsoft.EntityFrameworkCore;

namespace Practice_WebAPI_01.Repository;

public class WeaponRepository : IWeaponRepository
{
    private readonly DataContext _context;

    public WeaponRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteWeapon(Weapon weapon)
    {
        _context.Remove(weapon);
        return await Save();
    }

    public async Task<Weapon> GetWeapon(int weaponId)
    {
        return await _context.Weapons.FirstOrDefaultAsync(w => w.Id == weaponId);
    }

    public async Task<ICollection<Weapon>> GetWeapons()
    {
        return await _context.Weapons.ToListAsync();
    }

    public async Task<bool> RegisterWeapon(Weapon weapon)
    {
        await _context.AddAsync(weapon);
        return await Save();
    }

    public async Task<bool> Save()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }

    public async Task<bool> WeaponExists(string weaponName)
    {
        var x = await _context.Weapons.Where(w => w.Name.Trim().ToUpper() == weaponName.Trim().ToUpper()).FirstOrDefaultAsync();

        if (x != null)
            return true;

        return false;
    }
}
