namespace Markivio.Application.Dto;


public class ArticleInformation
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string? Description { get; set; } = null;
    public bool IsFramable { get; set; } = false;
    public UserInformation User { get; set; }
    public TagSoftInformation[] Tags { get; set; } = Array.Empty<TagSoftInformation>();
}

public record ArticleGetByName(
    string Name
    );


public record CreateArticle(
    string Title,
    string Source,
    string? Description,
    TagCreateArticle[] Tags
    );

public record AddTagsToArticle(
    Guid articleId,
    Guid[] tagIds
);

public record RemoveTagsToArticle(
    Guid articleId,
    Guid[] tagIds
);

public readonly record struct ArticleFilters(
    string? Title,
    List<string>? TagNames
);
