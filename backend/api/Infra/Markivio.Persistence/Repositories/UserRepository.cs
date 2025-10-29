using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Extensions.Identity;
using Markivio.Persistence.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;


namespace Markivio.Persistence.Repositories;

public class UserRepository(MarkivioContext context,
    IHttpClientFactory httpClientFactory,
    IMemoryCache cache) : GenericRepositpory<User>(context), IUserRepository
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
        JwtTokenInfo tok = JwtTokenExtentions.ParseToken(JwtToken);

        string baseUrl = tok.Audiences.ToArray()[1];
        string authId = tok.Subject;

        string keyCache = $"User_Token_{authId}";
        UserInfoFromAuth? userInfo = await cache.GetOrCreateAsync<UserInfoFromAuth?>(keyCache, async cacheEntry =>
        {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(baseUrl),
            };
            request.Headers.Add("Authorization", $"Bearer {JwtToken}");

            HttpResponseMessage responseMessage = await client.SendAsync(request, token);
            if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                throw new InvalidOperationException();

            return await responseMessage.Content.ReadFromJsonAsync<UserInfoFromAuth>(new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower,
            });
        });

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
