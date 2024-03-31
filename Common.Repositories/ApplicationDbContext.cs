using Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Common.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Todo> Tados { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
    public DbSet<ApplicationUserApplicationRole> ApplicationUserApplicationRole { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>().HasKey(t => t.Id);
        modelBuilder.Entity<Todo>().Property(l => l.Label).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Todo>().
            HasOne(v => v.User).
            WithMany(c => c.Todos).
            HasForeignKey(w => w.OwnerId);

        modelBuilder.Entity<ApplicationUser>().HasKey(u => u.Id);
        modelBuilder.Entity<ApplicationUser>().Property(s => s.Name).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<ApplicationUser>().HasIndex(u => u.Name).IsUnique();
        modelBuilder.Entity<ApplicationUser>().Navigation(e => e.Roles).AutoInclude();
        modelBuilder.Entity<ApplicationUserApplicationRole>().Navigation(n => n.ApplicationUserRole).AutoInclude();

        modelBuilder.Entity<RefreshToken>().HasKey(u => u.Id);
        modelBuilder.Entity<RefreshToken>().Property(u => u.Id).HasDefaultValueSql("uuid_generate_v4()");
        modelBuilder.Entity<RefreshToken>()
            .HasOne(e => e.ApplicationUser)
            .WithMany().HasForeignKey(e => e.ApplicationUserId);

        modelBuilder.Entity<ApplicationUserApplicationRole>().HasKey(c => new
        {
            c.ApplicationUserId,
            c.ApplicationUserRoleId
        });

        modelBuilder.Entity<ApplicationUser>().
            HasMany(u => u.Roles).
            WithOne(e => e.ApplicationUser).
            HasForeignKey(e => e.ApplicationUserId);

        modelBuilder.Entity<ApplicationUserRole>().
           HasMany(u => u.Users).
           WithOne(e => e.ApplicationUserRole).
           HasForeignKey(e => e.ApplicationUserRoleId);

        //modelBuilder.Entity<User>().;

        modelBuilder.Entity<ApplicationUserRole>().HasKey(u => u.Id);
        modelBuilder.Entity<ApplicationUserRole>().Property(s => s.Name).HasMaxLength(50).IsRequired();
        base.OnModelCreating(modelBuilder);
    }
}
