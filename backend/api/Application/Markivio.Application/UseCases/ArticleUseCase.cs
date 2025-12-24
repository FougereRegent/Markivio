using FluentResults;
using Markivio.Application.Dto;
using Markivio.Application.Errors;
using Markivio.Application.Mapper;
using Markivio.Domain.Auth;
using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;

namespace Markivio.Application.UseCases;

public interface IArticleUseCase
{
    ValueTask<Result<ArticleInformation>> GetById(Guid id, CancellationToken cancelationToken = default);
    ValueTask<Result<ArticleInformation>> CreateArticle(CreateArticle createArticle, CancellationToken cancellationToken = default);
    IQueryable<ArticleInformation> FindByFilter(ArticleFilters articleFilters);
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

        article = mapper.CreateArticleToArticle(createArticle);
        article.User = authUser.CurrentUser;

        if (!CheckIfTagsExits(createArticle))
            return Result.Fail(new NotFoundError("A tag doesn't exist"));

        TagMapper tagMapper = new TagMapper();
        article.ArticleContent.Tags = tagRepository.GetByIds(createArticle.Tags.Select(pre => pre.Id).ToList())
          .Select(pre => tagMapper.TagToSoftTag(pre)).ToList();

        Result result = article.Validate();
        if (result.IsFailed)
            return Result.Merge(result);

        Article resultArticle = articleRepository.Save(article);
        return mapper.ArticleToArticleInformation(resultArticle);
    }

    private bool CheckIfTagsExits(CreateArticle createArticle)
    {
        TagMapper tagMapper = new TagMapper();
        if (createArticle.Tags is null || createArticle.Tags is { Length: 0 })
            return true;
        Tag[] tags = createArticle.Tags.Select(pre => tagMapper.TagCreateArticleToTag(pre))
          .ToArray();

        return tagUseCase.TagsExist(tags);
    }
}
