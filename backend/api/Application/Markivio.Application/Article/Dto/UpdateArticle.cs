namespace Markivio.Application.Dto;

public record UpdateArticle(
        Guid Id,
        string Title,
        string? Description,
        TagArticle[] Tags
        );

