using Practice_WebAPI_01.Models;

namespace Practice_WebAPI_01.Interfaces;

public interface IWeaponTypeRepository: IRepository<WeaponType>
{
    Task<bool> UpdateWeaponType(WeaponType obj);

    //Task<WeaponType> GetFirstOrDefault(int weaponTypeId);
    //Task<ICollection<WeaponType>> GetAll();
    //Task<bool> Exists(string weaponTypeName);
    //void Add(WeaponType weaponType);
    //void Remove(WeaponType weaponType);
    //Task<bool> Save();
}
