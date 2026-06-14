using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;


namespace Markivio.Persistence.Repositories;

public class UserRepository(MarkivioContext context, IUnitOfWork unitOfWork) : GenericRepository<User>(context, unitOfWork), IUserRepository
{
    public async Task<User?> GetUserByAuthId(string authId, CancellationToken token = default)
    {
        return await _context.User.FirstOrDefaultAsync(pre => pre.AuthId == authId, token);
    }
}
