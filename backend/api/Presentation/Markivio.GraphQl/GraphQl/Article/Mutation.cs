using Markivio.Application.Dto;
using Markivio.Application.UseCases;

namespace Markivio.Presentation.GraphQl;

public partial class Mutation {
    public async Task<ArticleInformation> CreateArticle(IArticleUseCase articleUseCase,
        CreateArticle createArticle,
        CancellationToken cancellationToken = default)
    {
        FluentResults.Result<ArticleInformation> resultCreate = await articleUseCase.CreateArticle(createArticle, cancellationToken);
        return resultCreate.ThrowIfResultIsFailed();
    }

    public async Task<ArticleInformation> UpdateArticle(IArticleUseCase articleUseCase,
            UpdateArticle updateArticle,
            CancellationToken cancellationToken = default)
    {
        FluentResults.Result<ArticleInformation> resultUpdate = await articleUseCase.UpdateArticle(updateArticle, cancellationToken);
        return resultUpdate.ThrowIfResultIsFailed();
    }

    public async Task<ArticleInformation> AddTags(IArticleUseCase articleUseCase,
        AddTagsToArticle addTagsToArticle)
    {
        FluentResults.Result<ArticleInformation> resultAddTags = await articleUseCase.AddTags(addTagsToArticle);
        return resultAddTags.ThrowIfResultIsFailed();
    }
    public async Task<ArticleInformation> RemoveTags(IArticleUseCase articleUseCase,
        RemoveTagsToArticle removeTagsToArticle)
    {
        FluentResults.Result<ArticleInformation> resultRemoveTags = await articleUseCase.RemoveTags(removeTagsToArticle);
        return resultRemoveTags.ThrowIfResultIsFailed();
    }
}
