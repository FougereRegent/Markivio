using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Repositories;

public class GenericRepository<T>(MarkivioContext context) : IGenericRepository<T> where T : Entity
{
	protected MarkivioContext _context = context;

    public void Delete(T entity) =>
        context.Remove(entity);

    public IQueryable<T> GetAll() {
        return _context.Set<T>()
		 .AsNoTracking()
          .AsQueryable()
          .OrderBy(pre => pre.Id);
	}

    public async ValueTask<T?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        T? result = await _context.Set<T>()
			.AsNoTracking()
			.FirstOrDefaultAsync(pre => pre.Id == id, cancellationToken);
        return result;
    }

    public IQueryable<T> GetByIds(IEnumerable<Guid> ids)
    {
        return _context.Set<T>()
		.AsNoTracking()
          .Where(pre => ids.Contains(pre.Id));
    }

    public T Save(T entity) {
      return context.Add<T>(entity).Entity;
	}

    public void SaveInRange(IEnumerable<T> entities) {
      context.AddRange(entities);
	}

    public T Update(T entity) {
	  context.Attach(entity);
      return context.Update<T>(entity).Entity;
	}

    public void UpdateInRange(IEnumerable<T> entities){
	  context.AttachRange(entities);
      context.UpdateRange(entities);
	}
}
