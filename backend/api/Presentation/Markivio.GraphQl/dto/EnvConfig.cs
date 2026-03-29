namespace Markivio.Presentation.Dto;

public sealed class EnvConfig
{
    public string MARKIVIO_AUTHORITY { get; set; } = string.Empty;
    public string MARKIVIO_AUDIENCE { get; set; } = string.Empty;
    public string CONNECTION_STRING { get; set; } = string.Empty;
    public string MARKIVIO_CORS_ORIGIN { get; set; } = string.Empty;
}

