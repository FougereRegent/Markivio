namespace Markivio.Application.Dto;


public class ArticleInformation
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public UserInformation User { get; set; }
    public TagSoftInformation[] Tags { get; set; } = Array.Empty<TagSoftInformation>();
}

public record ArticleGetByName(
    string Name
    );


public record CreateArticle(
    string Title,
    string Source,
    TagCreateArticle[] Tags
    );
