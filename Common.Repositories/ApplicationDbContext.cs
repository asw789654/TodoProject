using Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Common.Repositories;

public class ApplicationDbContext : DbContext
{
    public DbSet<Todo> Tados { get; set; }
    public DbSet<Todo> Users { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>().HasKey(t => t.Id);
        modelBuilder.Entity<Todo>().Property(l => l.Label).HasMaxLength(100).IsRequired();
        
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(s => s.Name).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<User>().Property(d => d.Name).HasMaxLength(100).IsRequired();

        modelBuilder.Entity<Todo>().
            HasOne(v => v.User).
            WithMany(c=>c.Todos).
            HasForeignKey(w => w.OwnerId);


        base.OnModelCreating(modelBuilder);
    }
}
