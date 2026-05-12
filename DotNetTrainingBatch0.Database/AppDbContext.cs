using DotNetTrainingBatch0.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace DotNetTrainingBatch0.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> TblUsers { get; set; }
    public DbSet<AppRole> TblRoles { get; set; }
    public DbSet<AppPermission> TblPermissions { get; set; }
    public DbSet<AppRolePermission> TblRolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
