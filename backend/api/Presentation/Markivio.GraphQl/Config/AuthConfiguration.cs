using Auth0.AspNetCore.Authentication;
using Markivio.Presentation.Dto;

namespace Markivio.Presentation.Config;

public static class AuthConfiguration
{
    public static void AddAuth0(this IServiceCollection services, EnvConfig config)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
        services.AddAuthorization();
        services.AddAuth0WebAppAuthentication(options =>
        {
            options.CallbackPath = "/auth/callback";
            options.Domain = config.Domain;
            options.ClientId = config.ClientId;
        });
    }

    public static void UseAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
