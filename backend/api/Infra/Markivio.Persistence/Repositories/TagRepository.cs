using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;


namespace Markivio.Persistence.Repositories;

public class TagRepository(MarkivioContext context) : GenericRepositpory<Tag>(context), ITagRepository
{
    public async ValueTask<List<Tag>> SearchTagByName(string tagName, CancellationToken token = default)
    {
		return await _context.Tag.Where(pre => pre.Name.StartsWith(tagName))
			.OrderBy(pre => pre.Id)
			.Take(10)
			.ToListAsync(token);
    }
}
