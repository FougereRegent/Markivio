namespace Markivio.Domain.Entities;

public sealed class Tag : Entity, IModelValidation
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public User User { get; set; } = null!;

    public bool Validate()
    {
        throw new NotImplementedException();
    }
}
