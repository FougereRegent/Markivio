namespace Markivio.Domain.Entities;

public class EntityWithTenancy : Entity {
    public User User { get; set; } = null!;
}
