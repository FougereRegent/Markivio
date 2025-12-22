using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper(AllowNullPropertyAssignment = true)]
public partial class ArticleMapper
{
    [MapNestedProperties(nameof(Article.ArticleContent))]
    public partial ArticleInformation ArticleToArticleInformation(Article article);

    public Article CreateArticleToArticle(CreateArticle article)
    {
        return new Article
        {
            ArticleContent = new ArticleContent
            {
                Source = article.Source,
                Tags = article.Tags.Select(pre => new SoftTag { Name = pre.Name }).ToList()
            },
            Title = article.Title,
        };
    }
}

[Mapper]
public static partial class ArticleMapperProjection
{
    public static partial IQueryable<ArticleInformation> ProjectionToDto(this IQueryable<Article> articles);

    [MapNestedProperties(nameof(Article.ArticleContent))]
    private static partial ArticleInformation ArticleToArticleInformation(Article article);
}
