using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Repositories;

public class FolderRepository(MarkivioContext context) : GenericRepositpory<Folder>(context), IFolderRepository
{
}
