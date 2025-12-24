using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Repositories;

public class ArticleRepository(MarkivioContext context) : GenericRepositpory<Article>(context), IArticleRepository
{
    public async ValueTask<Article?> GetByTitle(string title)
    {
        return await context.Article.Where(pre => pre.Title == title)
            .FirstOrDefaultAsync();
    }

    public IQueryable<Article> Filter(string? title, List<string>? tagName)
    {
        IQueryable<Article> baseResult = context.Article.AsQueryable();
        IQueryable<Article> resultFitler = (title, tagName) switch
        {
            (null, List<string> a) when a is { Count: > 0 } => baseResult.Where(pre => pre.ArticleContent.Tags.Any(pre => a.Contains(pre.Name))),
            (string t, List<string> a) when a is { Count: > 0 } => baseResult.Where(pre => pre.ArticleContent.Tags.Any(pre => a.Contains(pre.Name)) && pre.Title == t),
            (string t, _) => baseResult.Where(pre => pre.Title == t),
            (_, _) => baseResult
        };

        return resultFitler;
    }
}
