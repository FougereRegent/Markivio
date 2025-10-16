namespace Markivio.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; set; }
}

public interface IModelValidation
{
    bool Validate();
}
