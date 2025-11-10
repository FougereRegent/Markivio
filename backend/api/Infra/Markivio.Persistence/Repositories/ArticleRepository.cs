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
}
