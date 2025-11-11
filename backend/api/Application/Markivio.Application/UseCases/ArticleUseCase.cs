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

public class ArticleUseCase(IArticleRepository articleRepository, IAuthUser authUser) : IArticleUseCase
{
    public IQueryable<ArticleInformation> GetArticles()
    {
        ArticleMapper articleInformation = new ArticleMapper();
        return articleRepository
          .GetAll()
          .ProjectionToDto()
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

        Result result = article.Validate();
        if (result.IsFailed)
            return Result.Merge(result);

        Article resultArticle = articleRepository.Save(article);
        return mapper.ArticleToArticleInformation(resultArticle);
    }

}
