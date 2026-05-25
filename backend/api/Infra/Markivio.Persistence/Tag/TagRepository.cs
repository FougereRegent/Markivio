using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;

namespace Markivio.Persistence.Repositories;

public class TagRepository(MarkivioContext context, IUnitOfWork unitOfWork) : GenericRepository<Tag>(context, unitOfWork), ITagRepository
{
}
