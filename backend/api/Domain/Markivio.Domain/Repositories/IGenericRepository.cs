using Markivio.Domain.Entities;
namespace Markivio.Domain.Repositories;

public interface IGenericRepository<T> where T : Entity
{
    void Delete(T entity);

    T Update(T entity);
    T Save(T entity);

    void UpdateInRange(IEnumerable<T> entities);
    void SaveInRange(IEnumerable<T> entities);

    IQueryable<T> GetAll();
    ValueTask<T?> GetById(Guid id);
    ValueTask<PaginatedValues<T>> GetAllPaginated(int limit, int skip);
}
