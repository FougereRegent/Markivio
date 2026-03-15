namespace Markivio.Presentation.Dto;

internal enum EnumHealthStatus
{
    Alive,
    Failed,
    Unknown,
}


internal record HealthCheckDto(
    EnumHealthStatus Status
    );

internal record VersionDto(
    string Name,
    string Version
    );

internal record DefaultMessageDto(
    string Message
    );
