using Practice_WebAPI_01.Models;

namespace Practice_WebAPI_01.Interfaces;

public interface IWeaponTypeRepository
{
    Task<WeaponType> GetWeaponType(int weaponTypeId);
    Task<ICollection<WeaponType>> GetWeaponTypes();
    Task<bool> WeaponTypeExists(string weaponTypeName);
    Task<bool> RegisterWeaponType(WeaponType weaponType);
    Task<bool> DeleteWeaponType(WeaponType weaponType);
    Task<bool> Save();

}
