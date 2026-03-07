using Markivio.Extensions.HostingExtensions;

namespace Markivio.Presentation.Dto;

public record EnvConfig(
    [EnvironmentVariable("MARKIVIO_AUTHORITY")] string Authority,
    [EnvironmentVariable("MARKIVIO_AUDIENCE")] string Audience,
    [EnvironmentVariable("MARKIVIO_CONNECTION_STRING")] string ConnectionString
);

