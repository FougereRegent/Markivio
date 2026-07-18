namespace Markivio.Application.Dto;

public record CreateArticle(
    string Title,
    string Source,
    string? Description,
    TagArticle[] Tags
    );
