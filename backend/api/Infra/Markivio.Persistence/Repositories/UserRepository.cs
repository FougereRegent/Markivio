using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;


namespace Markivio.Persistence.Repositories;

public class UserRepository(MarkivioContext context,
    IHttpClientFactory httpClientFactory) : GenericRepositpory<User>(context), IUserRepository
{
    class UserInfoFromAuth
    {
        public string GivenName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
    }

    public async ValueTask<User?> GetUserByAuthId(string authId, CancellationToken token = default) =>
       await context.User.FirstOrDefaultAsync(pre => pre.AuthId == authId, token);

    public async ValueTask<User?> GetUserInfoByToken(string JwtToken, CancellationToken token = default)
    {
        HttpClient client = httpClientFactory.CreateClient();
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken tok = handler.ReadJwtToken(JwtToken);

        string baseUrl = tok.Audiences.ToArray()[1];
        string authId = tok.Subject;

        HttpRequestMessage request = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(baseUrl),
        };
        request.Headers.Add("Authorization", $"Bearer {JwtToken}");

        HttpResponseMessage responseMessage = await client.SendAsync(request, token);
        if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine(responseMessage.StatusCode);
            throw new InvalidOperationException();
        }

        UserInfoFromAuth? userInfo = await responseMessage.Content.ReadFromJsonAsync<UserInfoFromAuth>();

        return new User
        {
            AuthId = authId,
            Email = userInfo?.Email ?? string.Empty,
            FirstName = userInfo?.GivenName ?? string.Empty,
            LastName = userInfo?.FamilyName ?? string.Empty,
            Username = userInfo?.NickName ?? string.Empty
        };
    }
}
