using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Markivio.Domain.ValueObject;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper(AllowNullPropertyAssignment = true, PreferParameterlessConstructors = false)]
public partial class ArticleMapper
{
	[MapNestedProperties(nameof(Article.ArticleContent))]
	public partial ArticleInformation Map(Article article);

	public Article Map(CreateArticle createArticle, List<TagValueObject> tags) {
		return new Article(
				articleContent: new ArticleContent(
					source: createArticle.Source,
					description: createArticle.Description,
					content: string.Empty,
					tags: tags
					),
				title: createArticle.Title
				);
	}
}

[Mapper]
public static partial class ArticleMapperProjection
{
    public static partial IQueryable<ArticleInformation> ProjectionToDto(this IQueryable<Article> articles);

    [MapNestedProperties(nameof(Article.ArticleContent))]
    private static partial ArticleInformation ArticleToArticleInformation(Article article);
}
