using DreamBig.Faist.Domain.Entities;
using DreamBig.Faist.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Task = DreamBig.Faist.Domain.Entities.Task;

namespace DreamBig.Faist.Persistence;

public class FaistDbContext(DbContextOptions<FaistDbContext> options) : DbContext(options)
{

    public DbSet<Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            entityEntry.Entity.UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Entity.CreatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
