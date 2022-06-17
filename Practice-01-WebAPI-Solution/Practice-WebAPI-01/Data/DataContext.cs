using Microsoft.EntityFrameworkCore;
using Practice_WebAPI_01.Models;

namespace Practice_WebAPI_01.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // Tables
        public DbSet <Hero> Heroes { get; set; }
        public DbSet <Weapon> Weapons { get; set; }
        public DbSet <WeaponType> WeaponTypes { get; set; }
    }
}
