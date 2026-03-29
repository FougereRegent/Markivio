using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Markivio.Domain.Entities;

namespace Markivio.Persistence;

public class DateTimeSaveUpdateInterceptor : SaveChangesInterceptor
{
    public DateTimeSaveUpdateInterceptor()
    {
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
            AuditEntity(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            AuditEntity(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void AuditEntity(DbContext dbContext)
    {
        foreach (var entry in dbContext.ChangeTracker.Entries<Entity>())
        {
            Action<EntityEntry<Entity>> func;
            switch (entry.State)
            {
                case EntityState.Added:
                    func = CreateEntity;
                    break;
                case EntityState.Modified:
                    func = ModifyEntity;
                    break;
                default:
                    continue;
            }
            func.Invoke(entry);
        }
    }

    private static void CreateEntity(EntityEntry<Entity> entry)
    {
        entry.Entity.CreatedAt = DateTimeOffset.UtcNow.ToUniversalTime();
        entry.Entity.UpdatedAt = DateTimeOffset.UtcNow.ToUniversalTime();
    }

    private static void ModifyEntity(EntityEntry<Entity> entry)
    {
        entry.Entity.UpdatedAt = DateTimeOffset.UtcNow.ToUniversalTime();
    }
}
