
namespace Markivio.Application.Dto;

public class TagInformation
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}

public record TagArticle(
    Guid Id
);

public record CreateTag(
    string Name,
    string Color
);

public class TagStats
{
    public string Name { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public int ArticleNumber { get; init; }

}
