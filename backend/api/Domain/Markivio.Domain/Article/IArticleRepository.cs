using Markivio.Domain.Entities;

namespace Markivio.Domain.Repositories;

public interface IArticleRepository : IGenericRepository<Article>
{
    Task<Article?> GetByTitle(string title, CancellationToken token = default!);
    IQueryable<Article> Filter(string? title, List<string>? tagName);
    Task<bool> IsFramable(string url, CancellationToken token = default!);
}
