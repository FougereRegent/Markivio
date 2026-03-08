using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Repositories;

public class TagRepository(MarkivioContext context, IDbContextFactory<MarkivioContext> factory) : GenericRepositpory<Tag>(context, factory), ITagRepository
{
}
