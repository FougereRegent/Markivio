using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;

namespace Markivio.Persistence.Repositories;

public class UserRepository(MarkivioContext context) : GenericRepositpory<User>(context), IUserRepository
{
}
