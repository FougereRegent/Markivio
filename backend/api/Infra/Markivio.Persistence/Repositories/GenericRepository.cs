using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Repositories;

public class GenericRepositpory<T>(MarkivioContext context) : IGenericRepository<T> where T : Entity
{
    public void Delete(T entity) =>
        context.Remove(entity);

    public IQueryable<T> GetAll() =>
        context.Set<T>()
          .AsQueryable();

    public async ValueTask<T?> GetById(Guid id)
    {
        T? result = await context.Set<T>().FirstOrDefaultAsync(pre => pre.Id == id);
        return result;
    }

    public T Save(T entity) =>
      context.Add<T>(entity).Entity;

    public void SaveInRange(IEnumerable<T> entities) =>
      context.AddRange(entities);

    public T Update(T entity) =>
      context.Update<T>(entity).Entity;

    public void UpdateInRange(IEnumerable<T> entities) =>
      context.UpdateRange(entities);
}
