#pragma warning disable RMG020
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

    [UserMapping]
    public Tag Map(CreateTag createTag) =>
        new(new TagValueObject(createTag.Name, createTag.Color));
}

[Mapper]
public static partial class TagMapperProjection
{
    public static partial IQueryable<TagInformation> ProjectionToTagInformation(this IQueryable<Tag> tags);

    [MapNestedProperties(nameof(Tag.TagValue))]
    private static partial TagInformation Map(Tag tag);
}

#pragma warning restore RMG020
