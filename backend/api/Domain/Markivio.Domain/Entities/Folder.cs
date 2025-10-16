namespace Markivio.Domain.Entities;

public sealed class Folder : Entity, IModelValidation
{
    public string Name { get; set; } = string.Empty;
    public List<Article> Articles { get; set; } = new List<Article>();
    public User User { get; set; } = null!;

    public bool Validate()
    {
        throw new NotImplementedException();
    }
}
