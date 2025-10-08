using Markivio.Presentation.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace Markivio.Presentation.Config;

public static class AuthConfiguration
{
    public static void AddAuth0(this IServiceCollection services, EnvConfig config)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = config.Authority;
            options.Audience = config.Audience;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateAudience = false,
            };
        });

        services.AddAuthorization(options =>
        {
        });
    }

    public static void UseAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
