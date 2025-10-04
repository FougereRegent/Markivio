using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Markivio.Presentation.Endpoints;

public static class AuthEndpoints
{
    private const string LOGIN_URL = "/auth/login";
    private const string PROFILE_URL = "/auth/profile";
    private const string CALLBACK_URL = "/auth/callback";

    public static void ConfigureAuthEndpoints(this WebApplication app)
    {
        app.MapGet(LOGIN_URL, LoginCallback);
        app.MapGet(PROFILE_URL, ProfileCallback);
        app.MapGet(CALLBACK_URL, CallbackUrlCallback);
    }

    private static async Task LoginCallback(HttpContext context, string returnUrl = "/")
    {
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
        // Indicate here where Auth0 should redirect the user after a login.
        // Note that the resulting absolute Uri must be added to the
        // **Allowed Callback URLs** settings for the app.
        .WithRedirectUri(returnUrl)
        .Build();

        await context.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    }

    private static async ValueTask<IResult> ProfileCallback(HttpContext context)
    {
        await Task.CompletedTask;
        return Results.Ok("bite");
    }

    private static async Task<IResult> CallbackUrlCallback(HttpContext context)
    {
        await Task.CompletedTask;
        return Results.Ok("test");
    }
}
