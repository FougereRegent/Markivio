namespace Markivio.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class EntityWithTenancy : Entity
{
    public User User { get; set; } = null!;
}

public interface IModelValidation
{
    void Validate();
}
