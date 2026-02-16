#pragma warning disable RMG020 
#pragma warning disable RMG012 
using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper(AllowNullPropertyAssignment = true)]
public partial class TagMapper
{
    public partial TagInformation TagToTagInformation(Tag tag);
    public partial Tag TagCreateArticleToTag(TagCreateArticle tag);
    public partial Tag CreateTagToTag(CreateTag tag);
    public partial SoftTag TagToSoftTag(Tag tag);
}

[Mapper]
public static partial class TagMapperProjection
{
    public static partial IQueryable<TagInformation> ProjectionToTagInformation(this IQueryable<Tag> tags);
    public static partial IQueryable<SoftTag> ProjectionToSoftTag(this IQueryable<Tag> tags);
}

#pragma warning restore RMG020
#pragma warning restore RMG012