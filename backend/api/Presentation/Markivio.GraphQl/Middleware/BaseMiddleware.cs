using HotChocolate.Resolvers;

namespace Markivio.Presentation.Middleware;

internal abstract class BaseMiddleware {
	private readonly FieldDelegate _next;

	public BaseMiddleware(FieldDelegate next) {
		_next = next;
	}

	public virtual async Task InvokeAsync(IMiddlewareContext context) {
		//before resolver
		await _next(context);
		//after resolver
	}
}
