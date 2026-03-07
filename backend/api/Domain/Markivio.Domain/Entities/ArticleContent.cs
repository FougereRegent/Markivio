
using FluentResults;

namespace Markivio.Domain.Entities;

public sealed class ArticleContent
{
    public string Source { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<SoftTag> Tags { get; set; } = new List<SoftTag>();
    public string? Description { get; set; } = null;
}

public sealed class SoftTag
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}
