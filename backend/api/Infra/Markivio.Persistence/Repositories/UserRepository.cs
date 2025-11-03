using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;


namespace Markivio.Persistence.Repositories;

public class UserRepository(MarkivioContext context) : GenericRepositpory<User>(context), IUserRepository
{

    public async ValueTask<User?> GetUserByAuthId(string authId, CancellationToken token = default) =>
       await context.User.FirstOrDefaultAsync(pre => pre.AuthId == authId, token);
}
