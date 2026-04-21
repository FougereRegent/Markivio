using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper(AllowNullPropertyAssignment = true, PreferParameterlessConstructors = false)]
public partial class ArticleMapper
{
    [MapNestedProperties(nameof(Article.ArticleContent))]
    public partial ArticleInformation Map(Article article);

    public Article Map(CreateArticle createArticle, List<Tag> tags, bool isFramable)
    {
        return new Article(
                articleContent: new ArticleContent(
                    source: createArticle.Source,
                    description: createArticle.Description,
                    content: string.Empty
                    ),
                title: createArticle.Title,
                isFramable: isFramable,
				tags: tags
                );
    }
}

[Mapper]
public static partial class ArticleMapperProjection
{
    public static partial IQueryable<ArticleInformation> ProjectionToArticleInformation(this IQueryable<Article> articles);

    [MapNestedProperties(nameof(Article.ArticleContent))]
    private static partial ArticleInformation Map(Article article);

	[MapNestedProperties(nameof(Tag.TagValue))]
	private static partial TagInformation Map(Tag tag);
}
