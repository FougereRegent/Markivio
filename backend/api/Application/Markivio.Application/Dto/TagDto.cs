
namespace Markivio.Application.Dto;

public class TagInformation
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}

public record TagCreateArticle(
    Guid Id
);

public record CreateTag(
    string Name,
    string Color
);
