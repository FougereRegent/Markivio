using System.IdentityModel.Tokens.Jwt;
namespace Markivio.Extensions.Identity;

public readonly record struct JwtTokenInfo(
    string Subject,
    string[] Audiences,
    string Issuer,
    string[] Scopes
    );

public static class JwtTokenExtentions
{
    public static JwtTokenInfo ParseToken(string token)
    {
        CleanToken(ref token);
        JwtSecurityTokenHandler securityToken = new JwtSecurityTokenHandler();
        JwtSecurityToken tok = securityToken.ReadJwtToken(token);

        string? strScopes = tok.Payload.GetValueOrDefault("scopes") as string;
        string[] scopes = [];
        if (strScopes is not null)
            scopes = strScopes.Split(' ');

        return new JwtTokenInfo(
            Subject: tok.Subject,
            Audiences: tok.Audiences.ToArray(),
            Issuer: tok.Issuer,
            Scopes: scopes
            );
    }

    private static void CleanToken(ref string token)
    {
        const string bearer = "Bearer ";
        if (token.Contains(bearer))
            token = token.Substring(bearer.Length);
    }
}
