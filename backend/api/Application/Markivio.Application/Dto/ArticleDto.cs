namespace Markivio.Application.Dto;


public record ArticleInformation(
    Guid Id,
    string Title,
    string Source,
    UserInformation User
    );
