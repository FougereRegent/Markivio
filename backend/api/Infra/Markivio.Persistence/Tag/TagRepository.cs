using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Repositories;

public class TagRepository(MarkivioContext context, IUnitOfWork unitOfWork) : GenericRepository<Tag>(context, unitOfWork), ITagRepository
{
    public IQueryable<TagStatistique> GetAllTagsWithStatistique() =>
        _context.Article.Include(pre => pre.Tags)
            .SelectMany(pre => pre.Tags)
            .GroupBy(pre => new
            {
                pre.Id,
                pre.TagValue.Name,
                pre.TagValue.Color,
            })
    .Select(pre => new TagStatistique
    {
        Id = pre.Key.Id,
        Name = pre.Key.Name,
        Color = pre.Key.Color,
        ArticleNumber = pre.Count()
    });
}
