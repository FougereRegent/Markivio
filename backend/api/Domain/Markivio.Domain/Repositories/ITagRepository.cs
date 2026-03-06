using Markivio.Domain.Entities;

namespace Markivio.Domain.Repositories;

public interface ITagRepository : IGenericRepository<Tag>
{
	ValueTask<List<Tag>> SearchTagByName(string tagName, CancellationToken token = default);
}
