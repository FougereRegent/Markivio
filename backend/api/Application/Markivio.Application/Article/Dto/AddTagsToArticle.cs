namespace Markivio.Application.Dto;

public record AddTagsToArticle(
    Guid articleId,
    Guid[] tagIds
);
