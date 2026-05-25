using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : Entity
{
    protected readonly MarkivioContext _context;
	protected readonly IUnitOfWork _unitOfWork;

    public GenericRepository(MarkivioContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
		_unitOfWork = unitOfWork;
    }

    public void Delete(T entity) =>
        _context.Remove(entity);

    public IQueryable<T> GetAll()
    {
        return _context.Set<T>()
          .AsQueryable()
          .OrderBy(pre => pre.Id);
    }

    public virtual async Task<T?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        T? result = await _context.Set<T>()
            .FirstOrDefaultAsync(pre => pre.Id == id, cancellationToken);
        return result;
    }

    public IQueryable<T> GetByIds(IEnumerable<Guid> ids)
    {
        return _context.Set<T>()
          .Where(pre => ids.Contains(pre.Id));
    }

    public T Save(T entity)
    {
        return _context.Add<T>(entity).Entity;
    }

    public async Task SaveAndCommit(CancellationToken token = default)
    {
		await _unitOfWork.SaveChangesAsync(token);

    }

    public void SaveInRange(IEnumerable<T> entities)
    {
        _context.AddRange(entities);
    }

    public T Update(T entity)
    {
        return _context.Update<T>(entity).Entity;
    }

    public void UpdateInRange(IEnumerable<T> entities)
    {
        _context.UpdateRange(entities);
    }
}
