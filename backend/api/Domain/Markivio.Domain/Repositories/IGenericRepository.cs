using Markivio.Domain.Entities;
namespace Markivio.Domain.Repositories;

public interface IGenericRepository<T> where T : Entity
{
    ValueTask<T> GetById(Guid id);
    ValueTask<List<T>> GetAll();
    ValueTask<T> Update(T entity);
    ValueTask<T> Save(T entity);
    ValueTask<T> Delete(T entity);

    ValueTask<T> GetAllPaginated(int limit, int skip);
}
