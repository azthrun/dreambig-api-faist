using DreamBig.Faist.Domain.Entities;
using DreamBig.Faist.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Task = DreamBig.Faist.Domain.Entities.Task;

namespace DreamBig.Faist.Persistence;

public class FaistDbContext(
    DbContextOptions<FaistDbContext> options
) : DbContext(options)
{

    public DbSet<Task> Tasks
    {
        get; set;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfiguration(new TaskConfiguration());

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseEntity>> entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseEntity>? entityEntry in entries)
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
