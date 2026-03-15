using Markivio.Domain.ValueObject;

namespace Markivio.Domain.Entities;

public sealed class Tag : EntityWithTenancy
{
	public TagValueObject TagValue {get;set;}

	private Tag() {}

	public Tag(TagValueObject tagValue) {
		TagValue = tagValue ?? throw new ArgumentNullException(nameof(tagValue));
	}
}
