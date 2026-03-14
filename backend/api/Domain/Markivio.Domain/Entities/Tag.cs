using Markivio.Domain.ValueObject;

namespace Markivio.Domain.Entities;

public sealed class Tag : EntityWithTenancy
{
    private const string REGEX_TAG_NAME = @"^[A-Za-zÀ-ÿà-ÿ\-\'’ 0-9 &#`\-_]{1,25}$";
    private const string REGEX_TAG_COLOR = @"^#[A-Fa-f0-9]{6}$";

	public TagValueObject TagValue {get;set;} = null!;

	public Tag(TagValueObject tagValue) {
		TagValue = tagValue;
	}
}
