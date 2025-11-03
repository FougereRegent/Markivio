using Markivio.Domain.Entities;

namespace Markivio.Domain.Auth;

public interface IAuthUser
{
    User CurrentUser { get; set; }
    ValueTask<User?> GetUserInfoByToken(string jwtToken, CancellationToken cancellationToken = default);
}
