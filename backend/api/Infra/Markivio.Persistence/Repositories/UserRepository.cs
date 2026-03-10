using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;


namespace Markivio.Persistence.Repositories;

public class UserRepository(MarkivioContext context) :GenericRepositpory<User>(context), IUserRepository
{

    public async ValueTask<User?> GetUserByAuthId(string authId, CancellationToken token = default) {
		Console.WriteLine("bite");
		return await _context.User.AsNoTracking().Where(pre => pre.AuthId == authId).FirstOrDefaultAsync(token);
	}
}
