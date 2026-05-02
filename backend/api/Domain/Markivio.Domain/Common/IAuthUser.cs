using Markivio.Domain.Entities;

namespace Markivio.Domain.Auth;

public interface IAuthUser
{
    User CurrentUser { get; set; }
    Task<User?> GetUserInfoByToken(string jwtToken, CancellationToken cancellationToken = default);
}
