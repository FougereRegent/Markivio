using Markivio.Presentation.Config;

namespace Markivio.Presentation.Dto;

public record EnvConfig(
    [EnvironmentVariable("MARKIVIO_DOMAIN")] string Domain,
    [EnvironmentVariable("MARKIVIO_CLIENTID")] string ClientId,
    [EnvironmentVariable("MARKIVIO_SECRET")] string Secret
);

