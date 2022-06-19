using Practice_WebAPI_01.Interfaces;

namespace Practice_WebAPI_01.Interfaces;

public interface IUnitOfWork
{
    //IHeroRepository Hero { get; }
    //IWeaponRepository Weapon { get; }
    IWeaponTypeRepository WeaponType { get; }
    Task<bool> Save();
}
