using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Markivio.Domain.Entities;
using Markivio.Domain.Auth;
using System.Linq.Expressions;

namespace Markivio.Persistence.Interceptors;

public sealed class UpdateTenancyInterceptor : SaveChangesInterceptor {
	private readonly IAuthUser authUser;
	public UpdateTenancyInterceptor(IAuthUser authService) {
		this.authUser = authUser;
	}

    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
		if(eventData.Context is not null) {
			UpdateTenancy(eventData.Context);
		}
        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
		if(eventData.Context is not null) {
			UpdateTenancy(eventData.Context);
		}
        return base.SavedChanges(eventData, result);
    }

	private void UpdateTenancy(DbContext context) {
		var entries = context.ChangeTracker.Entries<EntityWithTenancy>();
		foreach(var entry in entries) {
			if(entry.State == EntityState.Added) {
				entry.Property(nameof(EntityWithTenancy.User)).CurrentValue = this.authUser.CurrentUser;
			}
		}
	}
}
