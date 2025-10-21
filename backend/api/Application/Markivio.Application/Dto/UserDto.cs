namespace Markivio.Application.Dto;

public readonly record struct UserConnectionDto(
    string Email,
    string Name,
    string NickName,
    string AuthUserId
    );
