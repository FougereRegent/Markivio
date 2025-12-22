using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper]
public partial class TagMapper
{
    public partial TagInformation TagToTagInformation(Tag tag);
    public partial Tag TagCreateArticleToTag(TagCreateArticle tag);
}
