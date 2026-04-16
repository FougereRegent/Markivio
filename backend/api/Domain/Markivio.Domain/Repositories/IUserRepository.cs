using Markivio.Domain.Entities;

namespace Markivio.Domain.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUserByAuthId(string authId, CancellationToken token = default);
}
