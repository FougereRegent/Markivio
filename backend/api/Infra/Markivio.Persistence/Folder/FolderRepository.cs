using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;

namespace Markivio.Persistence.Repositories;

public class FolderRepository(MarkivioContext context, IUnitOfWork unitOfWork) : GenericRepository<Folder>(context, unitOfWork), IFolderRepository
{
}
