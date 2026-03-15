using FluentResults;
using Markivio.Application.Dto;
using Markivio.Application.Errors;
using Markivio.Application.Mapper;
using Markivio.Domain.Auth;
using Markivio.Domain.Entities;
using Markivio.Domain.Errors;
using Markivio.Domain.Exceptions;
using Markivio.Domain.Repositories;
using Markivio.Domain.ValueObject;

namespace Markivio.Application.UseCases;

public interface IArticleUseCase
{
    ValueTask<Result<ArticleInformation>> GetById(Guid id, CancellationToken cancelationToken = default);
    ValueTask<Result<ArticleInformation>> CreateArticle(CreateArticle createArticle, CancellationToken cancellationToken = default);
    IQueryable<ArticleInformation> FindByFilter(ArticleFilters articleFilters);

    ValueTask<Result<ArticleInformation>> AddTags(AddTagsToArticle addTags);
    ValueTask<Result<ArticleInformation>> RemoveTags(RemoveTagsToArticle removeTags);
}

public class ArticleUseCase(ITagUseCase tagUseCase, IArticleRepository articleRepository, ITagRepository tagRepository, IAuthUser authUser) : IArticleUseCase
{
    public ValueTask<Result<ArticleInformation>> GetById(Guid id, CancellationToken cancelationToken = default)
    {
        throw new NotImplementedException();
    }

    public IQueryable<ArticleInformation> FindByFilter(ArticleFilters articleFilters)
    {
        if (articleFilters is { Title: null, TagNames: null })
            return articleRepository.GetAll()
              .ProjectionToDto();

        return articleRepository.Filter(articleFilters.Title, articleFilters.TagNames)
          .ProjectionToDto();

    }

    public async ValueTask<Result<ArticleInformation>> CreateArticle(CreateArticle createArticle, CancellationToken cancellationToken = default)
    {
        ArticleMapper mapper = new ArticleMapper();
        Article? article = await articleRepository.GetByTitle(createArticle.Title);
        if (article is not null)
            return Result.Fail(new AlreadyExistError("This article already exist"));

        if (!CheckIfTagsExits(createArticle))
            return Result.Fail(new NotFoundError("A tag doesn't exist"));

        List<TagValueObject> tags = tagRepository
            .GetByIds(createArticle.Tags.Select(pre => pre.Id).ToList())
            .Select(pre => pre.TagValue).ToList();

        if (authUser.CurrentUser is null)
            return Result.Fail(new NullFieldError("User"));

        try
        {
            article = mapper.Map(createArticle, tags);
            article.User = authUser.CurrentUser;
        }
        catch (DomainException ex)
        {
            return Result.Fail(MapDomainException(ex));
        }

        Article resultArticle = articleRepository.Save(article);
        return mapper.Map(resultArticle);
    }

    public async ValueTask<Result<ArticleInformation>> AddTags(AddTagsToArticle addTags)
    {
        ArticleMapper mapper = new ArticleMapper();
        Article? article = await articleRepository.GetById(addTags.articleId);
        if (article is null)
            return Result.Fail(new NotFoundError("Article doesn't exist"));

        IReadOnlyList<TagValueObject> tags = tagRepository.GetByIds(addTags.tagIds)
            .Select(pre => pre.TagValue)
            .ToList();

        if (tags.Count() != addTags.tagIds.Length)
            return Result.Fail(new NotFoundError("Tags doesn't exist"));

        try
        {
            article.ArticleContent.AddTags(tags);
        }
        catch (DomainException ex)
        {
            return Result.Fail(MapDomainException(ex));
        }

        Article res = articleRepository.Update(article);
        return mapper.Map(res);
    }

    public async ValueTask<Result<ArticleInformation>> RemoveTags(RemoveTagsToArticle removeTags)
    {
        ArticleMapper mapper = new ArticleMapper();
        Article? article = await articleRepository.GetById(removeTags.articleId);
        if (article is null)
            return Result.Fail(new NotFoundError("Article doesn't exist"));

        IReadOnlyList<TagValueObject> tags = tagRepository.GetByIds(removeTags.tagIds)
            .Select(pre => pre.TagValue)
            .ToList();

        if (tags.Count != removeTags.tagIds.Length)
            return Result.Fail(new NotFoundError("Tags doesn't exist"));

        try
        {
            article.ArticleContent.RemoveTags(tags);
        }
        catch (DomainException ex)
        {
            return Result.Fail(MapDomainException(ex));
        }


        Article res = articleRepository.Update(article);
        return mapper.Map(res);
    }

    private bool CheckIfTagsExits(CreateArticle createArticle)
    {
        TagMapper tagMapper = new TagMapper();
        if (createArticle.Tags is null || createArticle.Tags is { Length: 0 })
            return true;

        Guid[] tags = createArticle.Tags.Select(pre => pre.Id)
          .ToArray();

        return tagUseCase.TagsExist<Guid>(tags, TagExistConditionEnum.Id);
    }

    private static Error MapDomainException(DomainException ex) =>
        ex.ErrorCode switch
        {
            "EMPTY_ARTICLESOURCE" => new ShouldNotBeEmptyError("Source"),
            "FORMAT_ARTICLE_SOURCE" => new FormatUnexpectedError("Source"),
            "EMPTY_ARTICLETITLE" => new ShouldNotBeEmptyError("Title"),
            "TAG_LIMIT_EXCEEDED" => new ExceedElementsError(20, "Tags"),
            _ => new Error(ex.Message)
        };
}
