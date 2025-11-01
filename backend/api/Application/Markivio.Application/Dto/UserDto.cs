namespace Markivio.Application.Dto;

public readonly record struct UserConnectionDto(
    string Token
    );

public readonly record struct UserInformation(
    Guid Id,
    string FirstName,
    string LastName,
    string Email
    );

public readonly record struct UpdateUserInformation(
    Guid Id,
    string FirstName,
    string LastName,
    string NickName
    );
