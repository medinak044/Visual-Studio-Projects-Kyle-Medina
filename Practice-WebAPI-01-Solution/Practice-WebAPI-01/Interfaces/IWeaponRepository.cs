using Practice_WebAPI_01.Models;

namespace Practice_WebAPI_01.Interfaces;

public interface IWeaponRepository
{
    Task<Weapon> GetWeapon(int weaponId);
    Task<ICollection<Weapon>> GetWeapons();
    Task<bool> WeaponExists(string weaponName);
    Task<bool> RegisterWeapon(Weapon weapon);
    Task<bool> DeleteWeapon(Weapon weapon);
    Task<bool> Save();

}
