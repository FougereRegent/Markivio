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
