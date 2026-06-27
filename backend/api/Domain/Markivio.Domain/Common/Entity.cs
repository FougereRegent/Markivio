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

public class EntityWithSoftDeleteAndTenancy : EntityWithTenancy {
	public bool IsRemoved { get; set; }
}

public interface IModelValidation
{
    void Validate();
}
