namespace Markivio.Presentation.Dto;

public sealed class EnvConfig
{
    //Auth Config
    public string MARKIVIO_AUTHORITY { get; set; } = string.Empty;
    public string MARKIVIO_AUDIENCE { get; set; } = string.Empty;

    //API Config
    public string MARKIVIO_CORS_ORIGIN { get; set; } = string.Empty;

    //DB Config
    public string MARKIVIO_DB_USER { get; set; } = string.Empty;
    public string MARKIVIO_DB_PASSWORD { get; set; } = string.Empty;
    public string MARKIVIO_DB_HOST { get; set; } = string.Empty;
    public string MARKIVIO_DB_PORT { get; set; } = "5432";

    public string CONNECTION_STRING { get; set; } = string.Empty;
}

