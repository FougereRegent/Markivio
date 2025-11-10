using Markivio.Domain.Entities;

namespace Markivio.Domain.Repositories;

public interface IArticleRepository : IGenericRepository<Article>
{
    ValueTask<Article?> GetByTitle(string title);
}
