using FluentResults;

namespace Markivio.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; set; }
	public DateTimeOffset UpdatedAt { get; set; }
	public DateTimeOffset CreatedAt { get; set; }
}

public interface IModelValidation
{
    Result Validate();
}
