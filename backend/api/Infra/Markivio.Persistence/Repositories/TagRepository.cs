using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;

namespace Markivio.Persistence.Repositories;

public class TagRepository(MarkivioContext context) : GenericRepositpory<Tag>(context), ITagRepository
{
}
