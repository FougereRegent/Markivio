using FluentResults;
using Markivio.Domain.Auth;
using Markivio.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Markivio.Extensions.Identity;
using System.Net.Http.Json;

namespace Markivio.Auth;

public class AuthUser(
    IHttpClientFactory httpClientFactory,
    IMemoryCache cache) : IAuthUser
{
    class UserInfoFromAuth
    {
        public string GivenName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
    }

    public User CurrentUser { get; set; } = default!;

    public async ValueTask<User?> GetUserInfoByToken(string jwtToken, CancellationToken cancellationToken = default)
    {
        HttpClient client = httpClientFactory.CreateClient();
        JwtTokenInfo tok = JwtTokenExtentions.ParseToken(jwtToken);

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
            request.Headers.Add("Authorization", $"Bearer {jwtToken}");

            HttpResponseMessage responseMessage = await client.SendAsync(request, cancellationToken);
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
