namespace Markivio.Application.Dto;


public record ArticleInformation(
    Guid Id,
    string Title,
    string Source,
    UserInformation User
    );

public record ArticleGetByName(
    string Name
    );


public record CreateArticle(
    string Title,
    string Source
    );
