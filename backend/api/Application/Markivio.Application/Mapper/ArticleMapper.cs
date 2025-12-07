using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper(AllowNullPropertyAssignment = true)]
public partial class ArticleMapper
{
    [MapNestedProperties(nameof(Article.ArticleContent))]
    public partial ArticleInformation ArticleToArticleInformation(Article article);

    [MapProperty(nameof(CreateArticle.Source), [nameof(Article.ArticleContent), nameof(Article.ArticleContent.Source)])]
    public partial Article CreateArticleToArticle(CreateArticle article);
}

[Mapper]
public static partial class ArticleMapperProjection
{
    public static partial IQueryable<ArticleInformation> ProjectionToDto(this IQueryable<Article> articles);

    [MapNestedProperties(nameof(Article.ArticleContent))]
    private static partial ArticleInformation ArticleToArticleInformation(Article article);
}
