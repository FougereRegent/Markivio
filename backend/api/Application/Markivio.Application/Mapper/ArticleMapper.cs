using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper(AllowNullPropertyAssignment = true)]
public partial class ArticleMapper
{
    public partial ArticleInformation ArticleToArticleInformation(Article article);
    public partial Article CreateArticleToArticle(CreateArticle article);
}

[Mapper]
public static partial class ArticleMapperProjection
{
    public static partial IQueryable<ArticleInformation> ProjectionToDto(this IQueryable<Article> articles);
}
