using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oversee.Models;

namespace Oversee.Data;

public class AppDbContext: IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
    #region Tables
    public DbSet<AppUser> AppUsers{ get; set; }
    public DbSet<Item> Items{ get; set; }
    public DbSet<ItemRecord> ItemRecords{ get; set; }
    public DbSet<ItemRecordUser> ItemRecordUsers{ get; set; }
    public DbSet<ItemRequest> ItemRequests{ get; set; }
    public DbSet<UserConnectionRequest> UserConnectionRequests{ get; set; }
    public DbSet<UserConnectionRequest_User> UserConnectionRequest_Users { get; set; }
    #endregion
}
