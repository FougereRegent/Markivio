using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;

namespace Markivio.Persistence.Repositories;

public class ArticleRepository(MarkivioContext context) : GenericRepositpory<Article>(context), IArticleRepository
{
}
