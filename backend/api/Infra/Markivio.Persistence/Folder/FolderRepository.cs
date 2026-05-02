using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;

namespace Markivio.Persistence.Repositories;

public class FolderRepository(MarkivioContext context) : GenericRepository<Folder>(context), IFolderRepository
{
}
