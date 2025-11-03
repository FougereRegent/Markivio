using FluentResults;
using Markivio.Application.Dto;
using Markivio.Application.Mapper;
using Markivio.Domain.Repositories;

namespace Markivio.Application.UseCases;

public interface IArticleUseCase
{
    IQueryable<ArticleInformation> GetArticles();
    ValueTask<Result<ArticleInformation>> GetById(Guid id, CancellationToken token = default);
    ValueTask<Result<ArticleInformation>> GetByName(ArticleGetByName article, CancellationToken token = default);
}

public class ArticleUseCase(IArticleRepository articleRepository) : IArticleUseCase
{
    public IQueryable<ArticleInformation> GetArticles()
    {
        ArticleMapper articleInformation = new ArticleMapper();
        return articleRepository
          .GetAll()
          .Select(pre => articleInformation.ArticleToArticleInformation(pre));
    }

    public ValueTask<Result<ArticleInformation>> GetById(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<ArticleInformation>> GetByName(ArticleGetByName article, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
