using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;


namespace Markivio.Persistence.Repositories;

public class UserRepository(MarkivioContext context, IDbContextFactory<MarkivioContext> factory) : GenericRepositpory<User>(context, factory), IUserRepository
{

    public async ValueTask<User?> GetUserByAuthId(string authId, CancellationToken token = default) {
		Console.WriteLine("bite");
		MarkivioContext db = _factory.CreateDbContext();
       return await db.User.AsNoTracking().Where(pre => pre.AuthId == authId).FirstOrDefaultAsync();
	}
}
