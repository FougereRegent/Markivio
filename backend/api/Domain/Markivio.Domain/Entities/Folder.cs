namespace Markivio.Domain.Entities;

public sealed class Folder : EntityWithTenancy
{
    public string Name { get; set; } = string.Empty;
    public List<Article> Articles { get; set; } = new List<Article>();
}
