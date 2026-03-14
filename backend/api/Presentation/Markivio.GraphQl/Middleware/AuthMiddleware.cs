using HotChocolate.Resolvers;
using Markivio.Domain.Entities;
using Markivio.Domain.Auth;


namespace Markivio.Presentation.Middleware;

internal class AuthMiddleware {
	private readonly FieldDelegate _next;

	public AuthMiddleware(FieldDelegate next) {
		_next = next;
	}

	public async Task InvokeAsync(IMiddlewareContext context)
    {
		User user = context.GetGlobalState<User>("auth-user");
		IAuthUser authService = context.Services.GetRequiredService<IAuthUser>();
		authService.CurrentUser = user;
		await _next(context);
    }
}
