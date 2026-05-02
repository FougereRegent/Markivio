using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore.Storage;

namespace Markivio.Persistence;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync(CancellationToken token = default);
    Task SaveChangesAsync(CancellationToken token = default);
    Task RollbackChangesAsync(CancellationToken token = default);
}

public class UnitOfWork(MarkivioContext dbcontext) : IUnitOfWork
{
    private IDbContextTransaction? transaction = null;

    public async Task BeginTransactionAsync(CancellationToken token = default)
    {
        if (transaction is not null)
        {
            await transaction.RollbackAsync(token);
            await transaction.DisposeAsync();
            transaction = null;
        }
        transaction = await dbcontext.Database.BeginTransactionAsync(token);
    }

    public void Dispose()
    {
        transaction?.Dispose();
    }

    public async Task RollbackChangesAsync(CancellationToken token = default)
    {
        if (transaction is null)
            throw new InvalidOperationException("You cannot rollback a transaction when a transaction has not opened");

        await transaction.RollbackAsync(token);
    }

    public async Task SaveChangesAsync(CancellationToken token = default)
    {
        if (transaction is null)
            throw new InvalidOperationException("You cannot save changes when a transaction has not opened");

        await dbcontext.SaveChangesAsync(token);
        await transaction.CommitAsync(token);

        await transaction.DisposeAsync();
        transaction = null;
    }
}
