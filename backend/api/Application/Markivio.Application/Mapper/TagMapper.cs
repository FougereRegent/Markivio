#pragma warning disable RMG020 
#pragma warning disable RMG012 
using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Markivio.Domain.ValueObject;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper(AllowNullPropertyAssignment = true, PreferParameterlessConstructors = false)]
public partial class TagMapper
{
	[MapNestedProperties(nameof(Tag.TagValue))]
    public partial TagInformation MapToTagInformation(Tag tag);

	[MapNestedProperties(nameof(Tag.TagValue))]
	public partial TagSoftInformation MapToSoftInformation(Tag tag);

	[UserMapping]
	public Tag Map(CreateTag createTag) =>
		new(new TagValueObject(createTag.Name, createTag.Color));
}

[Mapper]
public static partial class TagMapperProjection
{
    public static partial IQueryable<TagInformation> ProjectionToTagInformation(this IQueryable<Tag> tags);

	[MapNestedProperties(nameof(Tag.TagValue))]
	private static partial TagInformation TagToTagInformation(Tag tag);
}

#pragma warning restore RMG020
#pragma warning restore RMG012
