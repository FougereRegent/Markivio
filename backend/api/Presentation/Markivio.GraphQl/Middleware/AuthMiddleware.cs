using HotChocolate.Resolvers;
using Markivio.Domain.Entities;
using Markivio.Domain.Auth;


namespace Markivio.Presentation.Middleware;

internal sealed class AuthMiddleware(FieldDelegate next) : BaseMiddleware(next)
{
    public override async Task InvokeAsync(IMiddlewareContext context)
    {
        User user = context.GetGlobalState<User>("auth-user");
        IAuthUser authService = context.Services.GetRequiredService<IAuthUser>();
        authService.CurrentUser = user;

        await base.InvokeAsync(context);
    }
}
