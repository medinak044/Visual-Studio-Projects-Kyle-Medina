using Practice_WebAPI_01.Interfaces;
using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Practice_WebAPI_01.Repository;

public class WeaponTypeRepository: Repository<WeaponType>, IWeaponTypeRepository
{
    private readonly DataContext _context;

    public WeaponTypeRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    // See video below to create override functions from Repository in case of logging, for example
    // https://www.youtube.com/watch?v=-jcf1Qq8A-4

    // Refactor as a generic function in Repository?
    //^ No, because we also need to account for the model's navigation properties as well
    public async Task<bool> UpdateWeaponType(WeaponType obj)
    {
        var objFromDb = await _context.WeaponTypes.FirstOrDefaultAsync(w => w.Id == obj.Id);

        if (objFromDb == null)
            return false;

        //#region Map values from obj if they are new
        //objFromDb.Id = obj.Id;
        //objFromDb.Type = obj.Type;
        //#endregion

        //var weaponTypeMap = _mapper.Map<WeaponType>(objFromDb);

        return true; // Remember to call a Save() method afterwards to commit these changes to db
    }

    //public async Task<WeaponType> GetFirstOrDefault(int weaponTypeId)
    //{
    //    return await _context.WeaponTypes.FirstOrDefaultAsync(w => w.Id == weaponTypeId);
    //}

    ////NOTE: GetAll() could be expanded to set a limit for the amount of records being retrieved.(ex. 1000 returns top 1000, -1 returns everything)
    //public async Task<ICollection<WeaponType>> GetAll()
    //{
    //    return await _context.WeaponTypes.ToListAsync();
    //}
    //public async Task<bool> Exists(string weaponTypeName)
    //{
    //    var x = await _context.WeaponTypes.Where(w => w.Type.Trim().ToUpper() == weaponTypeName.Trim().ToUpper()).FirstOrDefaultAsync();

    //    if (x != null)
    //        return true;

    //    return false;
    //}

    //public void Add(WeaponType weaponType)
    //{
    //    _context.Add(weaponType);
    //    //return await Save();
    //}
    //public void Remove(WeaponType weaponType)
    //{
    //    _context.Remove(weaponType);
    //    //return await Save();
    //}

    ////public async Task<bool> Save()
    ////{
    ////    var saved = await _context.SaveChangesAsync();
    ////    return saved > 0 ? true : false;
    ////}
}
