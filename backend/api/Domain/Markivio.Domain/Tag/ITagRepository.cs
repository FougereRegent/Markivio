using Markivio.Domain.Entities;

namespace Markivio.Domain.Repositories;

public class TagStatistique
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public int ArticleNumber { get; init; }
}

public interface ITagRepository : IGenericRepository<Tag>
{
    IQueryable<TagStatistique> GetAllTagsWithStatistique();
}
