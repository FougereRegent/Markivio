namespace Markivio.Presentation.Dto;

public record EnvConfig(
    string Authority,
    string Audience,
    string ConnectionString,
	string CorsOrigin
);

