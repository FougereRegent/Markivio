using Markivio.Presentation.Dto;
using Microsoft.AspNetCore.Authorization;

namespace Markivio.Presentation.Endpoints;

public static class EndpointStatus
{
    public static void ConfigureStatusEndpoints(this WebApplication app)
    {
        app.MapGet("/", () =>
        {
            return new DefaultMessageDto("test");
        })
        .WithDisplayName("Default Route")
        .WithDescription("The default route");

        app.MapGet("/version", () =>
        {
            return new VersionDto("markivio", "v0.0.0");
        })
        .WithDisplayName("Version")
        .WithDescription("Get api version");


        app.MapGet("/health-check", [Authorize] () =>
        {
            return Task.FromResult(Results.Ok(new HealtkCheckDto(EnumHealthStatus.Alive)));
        })
        .WithDisplayName("Health Check")
        .WithDescription("Get api health-check");
    }
}
