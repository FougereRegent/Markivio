using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Repositories;

public class ArticleRepository(MarkivioContext context, HttpClient httpClient) : GenericRepository<Article>(context), IArticleRepository
{
    private readonly HttpClient _httpClient = httpClient;

    public override async Task<Article?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
		Article? result = await _context.Set<Article>()
			.Include(pre => pre.Tags)
			.FirstOrDefaultAsync(pre => pre.Id == id, cancellationToken);
        return result;
    }

    public async Task<Article?> GetByTitle(string title, CancellationToken token = default!)
    {
        return await _context.Article.Where(pre => pre.Title == title)
            .FirstOrDefaultAsync(token);
    }

    public IQueryable<Article> Filter(string? title, List<string>? tagName)
    {
        IQueryable<Article> baseResult = _context.Article.AsQueryable();
        IQueryable<Article> resultFitler = (title, tagName) switch
        {
            (null, List<string> a) when a is { Count: > 0 } => baseResult.Where(pre => pre.Tags.Any(pre => a.Contains(pre.TagValue.Name))),
            (string t, List<string> a) when a is { Count: > 0 } => baseResult.Where(pre => pre.Tags.Any(pre => a.Contains(pre.TagValue.Name)) && EF.Functions.ILike(pre.Title, $"{title}%")),
            (string t, _) => baseResult.Where(pre => EF.Functions.ILike(pre.Title, $"{title}%")),
            (_, _) => baseResult
        };

        return resultFitler.OrderBy(pre => pre.Id);
    }

    public async Task<bool> IsFramable(string url, CancellationToken token = default!)
    {
        const string framableHeader = "x-frame-options";
        var result = await _httpClient.GetAsync(url, token);
        return !result.Headers.Any(pre => pre.Key.Equals(framableHeader, StringComparison.InvariantCultureIgnoreCase));
    }
}
