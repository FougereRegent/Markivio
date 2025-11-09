using FluentResults;

namespace Markivio.Domain.Entities;

public sealed class Article : Entity, IModelValidation
{
    public string Title { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public Folder? Folder { get; set; } = null;
    public List<Tag> Tags { get; set; } = new List<Tag>();

    public Result Validate()
    {
        throw new NotImplementedException();
    }
}
