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
    IQueryable<ArticleInformation> GetArticles();
    ValueTask<Result<ArticleInformation>> GetById(Guid id, CancellationToken cancelationToken = default);
    ValueTask<Result<ArticleInformation>> GetByName(ArticleGetByName article, CancellationToken cancelationToken = default);
    ValueTask<Result<ArticleInformation>> CreateArticle(CreateArticle createArticle, CancellationToken cancellationToken = default);
}

public class ArticleUseCase(ITagUseCase tagUseCase, IArticleRepository articleRepository, ITagRepository tagRepository, IAuthUser authUser) : IArticleUseCase
{
    public IQueryable<ArticleInformation> GetArticles()
    {
        ArticleMapper articleInformation = new ArticleMapper();
        return articleRepository
          .GetAll()
          .ProjectionToDto()
          //.Select(pre => articleInformation.ArticleToArticleInformation(pre))
          .AsQueryable();
    }

    public ValueTask<Result<ArticleInformation>> GetById(Guid id, CancellationToken cancelationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<ArticleInformation>> GetByName(ArticleGetByName article, CancellationToken cancelationToken = default)
    {
        throw new NotImplementedException();
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

        Result result = article.Validate();
        if (result.IsFailed)
            return Result.Merge(result);

        TagMapper tagMapper = new TagMapper();
        article.ArticleContent.Tags = tagRepository.GetByIds(createArticle.Tags.Select(pre => pre.Id).ToList())
          .Select(pre => tagMapper.TagToSoftTag(pre)).ToList();

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
