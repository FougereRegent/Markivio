namespace Markivio.Presentation.Dto;

internal enum EnumHealthStatus
{
    Alive,
    Failed,
    Unknow,
}


internal record HealtkCheckDto(
    EnumHealthStatus Status
    );

internal record VersionDto(
    string Name,
    string Version
    );

internal record DefaultMessageDto(
    string Message
    );

