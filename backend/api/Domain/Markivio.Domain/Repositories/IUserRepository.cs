using Markivio.Domain.Entities;

namespace Markivio.Domain.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    ValueTask<User?> GetUserByAuthId(string authId, CancellationToken token = default);
    ValueTask<User?> GetUserInfoByToken(string JwtToken, CancellationToken token = default);
}
