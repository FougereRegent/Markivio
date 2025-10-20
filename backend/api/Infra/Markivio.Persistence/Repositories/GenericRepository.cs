using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Repositories;

public class GenericRepositpory<T>(MarkivioContext context) : IGenericRepository<T> where T : Entity
{
    public void Delete(T entity) =>
        context.Remove(entity);

    public async
      ValueTask<PaginatedValues<T>> GetAllPaginated(int limit, int skip)
    {
        Task<int> t_totaElement = context.Set<T>()
          .CountAsync();
        Task<List<T>> t_values = context.Set<T>()
          .Take(limit)
          .Skip(skip)
          .ToListAsync();

        int totalElement = await t_totaElement;
        List<T> values = await t_values;

        int pageSize = limit;
        int pageNumber = (skip / limit) + 1;
        int totalPage = (int)Math.Floor((decimal)(totalElement / limit));
        PaginatedValues<T> result = new PaginatedValues<T>(values, pageSize, pageNumber, totalPage);
        return result;
    }

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
