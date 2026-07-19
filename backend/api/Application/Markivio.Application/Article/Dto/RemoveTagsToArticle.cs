namespace Markivio.Application.Dto;

public record RemoveTagsToArticle(
    Guid articleId,
    Guid[] tagIds
);

