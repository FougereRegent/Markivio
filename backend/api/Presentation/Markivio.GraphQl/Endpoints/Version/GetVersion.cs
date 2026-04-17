using System.Reflection;

namespace Markivio.Presentation.Endpoint.Version;

public static class VersionEndpoint
{
    extension(RouteGroupBuilder route)
    {
        public RouteGroupBuilder GetVersion()
        {
            var versionRoute = route.MapGroup("/version");

            versionRoute.MapGet("/", () =>
            {
                var assembly = Assembly.GetExecutingAssembly();
                var informationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? string.Empty;
                var version = assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version ?? string.Empty;

                var versionDto = new VersionDto(
                        Version: version,
                        InformationalVersion: informationalVersion
                        );
                return Results.Ok(versionDto);
            })
            .WithName("Get Version")
            .WithDisplayName("Get Version")
            .WithDescription("Get Version on current API")
            .WithSummary("Get Version on current API")
            .Produces<VersionDto>(200);

            return route;
        }
    }
}
