namespace Markivio.Application.Dto;

public readonly record struct UserConnectionDto(
    string Email,
    string Name,
    string NickName,
    string AuthUserId
    );

public readonly record struct UserInformation(
    Guid Id,
    string FirstName,
    string LastName,
    string Email
    );
