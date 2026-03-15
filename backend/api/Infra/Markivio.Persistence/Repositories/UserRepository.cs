using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;


namespace Markivio.Persistence.Repositories;

public class UserRepository(MarkivioContext context) :GenericRepository<User>(context), IUserRepository
{
    public async ValueTask<User?> GetUserByAuthId(string authId, CancellationToken token = default) {
		return await _context.User.FirstOrDefaultAsync(pre => pre.AuthId == authId, token);
	}
}
