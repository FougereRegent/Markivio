namespace Markivio.Presentation.Endpoint.Config;

public static class ConfigEndpoint {
	private static WeakReference<ConfigFrontendDto> _weakConfig = new WeakReference<ConfigFrontendDto>(null!);
	private static object _lock = new object();

	extension(RouteGroupBuilder route) {
		public RouteGroupBuilder GetConfig() {
			var configRoute = route.MapGroup("/config");
			configRoute.MapGet("/",(IConfiguration configuration) => {
				if(_weakConfig.TryGetTarget(out var config) && config != null) {
					return Results.Ok(config);
				}
				lock(_lock) {
					config = new ConfigFrontendDto(
							AuthClientId: configuration.GetValue<string>("MARKIVIO_AUTH_ID", string.Empty),
							AuthDomain: configuration.GetValue<string>("MARKIVIO_AUTH_DOMAIN", string.Empty),
							AuthAudience: configuration.GetValue<string>("MARKIVIO_AUTH_AUDIENCE", string.Empty)
							);

					_weakConfig.SetTarget(config);
				}
				return Results.Ok(config);
			})
			.WithName("Get Config")
			.WithDisplayName("Get Config")
			.WithDescription("Get config for frontend")
			.WithSummary("Get config for frontend")
			.Produces<ConfigFrontendDto>(200);
			return route;
		}
	}
}
