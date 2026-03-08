using HotChocolate.Resolvers;
using Markivio.Domain.Entities;
using Markivio.Domain.Auth;

namespace Markivio.Presentation.Midleware;

public class AuthentificationMiddleware 
{
	private readonly FieldDelegate _next;

	public AuthentificationMiddleware(FieldDelegate next) {
		_next = next;
	}

	public async Task InvokeAsync(IMiddlewareContext context) {
		User user = context.GetGlobalState<User>("auth-user");
		IAuthUser authService = context.Services.GetRequiredService<IAuthUser>();
		Markivio.Persistence.Config.MarkivioContext db = context.Services.GetRequiredService<Markivio.Persistence.Config.MarkivioContext>();
		authService.CurrentUser = user;

		_next?.Invoke(context);
	}
}
