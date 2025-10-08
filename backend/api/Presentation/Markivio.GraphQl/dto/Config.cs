using Markivio.Presentation.Config;

namespace Markivio.Presentation.Dto;

public record EnvConfig(
    [EnvironmentVariable("MARKIVIO_AUTHORITY")] string Authority,
    [EnvironmentVariable("MARKIVIO_AUDIENCE")] string Audience
);

