using Markivio.Application.Dto;
using Markivio.Application.UseCases;
using Markivio.Presentation.GraphQl.Midleware;

namespace Markivio.Presentation.GraphQl;

public class Mutation
{
    public async ValueTask<UserInformation> UpdateMyUser(IUserUseCase userUseCase,
        UpdateUserInformation updateUserInformation,
        CancellationToken cancellationToken = default)
    {
        FluentResults.Result<UserInformation> resultUpdate = await userUseCase.UpdateCurrentUser(updateUserInformation);
        if (resultUpdate.IsFailed)
        {
            throw new GraphQLException(ErrorBuilder
                .New()
                .SetMessage(string.Join(Environment.NewLine, resultUpdate.Errors.Select(pre => pre.Message)))
                .Build()
                );
        }
        return resultUpdate.Value;
    }

    public async ValueTask<ArticleInformation> CreateArticle(IArticleUseCase articleUseCase,
        CreateArticle createArticle,
        CancellationToken cancellationToken = default)
    {
        FluentResults.Result<ArticleInformation> resultCreate = await articleUseCase.CreateArticle(createArticle, cancellationToken);
        if (resultCreate.IsFailed)
        {
            throw new GraphQLException(
                ErrorBuilder
                .New()
                .SetMessage(string.Join(Environment.NewLine, resultCreate.Errors.Select(pre => pre.Message)))
                .Build());
        }

        return resultCreate.Value;
    }

    public async ValueTask<TagInformation[]> CreateTags(ITagUseCase tagUseCase,
        List<CreateTag> createTags,
        CancellationToken cancellationToken = default)
    {
        FluentResults.Result<TagInformation[]> resultCreate = tagUseCase.CreateTag(createTags.ToArray());
        if (resultCreate.IsFailed)
        {
            throw new GraphQLException(
                ErrorBuilder
                .New()
                .SetMessage(string.Join(Environment.NewLine, resultCreate.Errors.Select(pre => pre.Message)))
                .Build());
        }

        return resultCreate.Value;
    }

    public async ValueTask<ArticleInformation> AddTags(IArticleUseCase articleUseCase,
        AddTagsToArticle addTagsToArticle)
    {
        FluentResults.Result<ArticleInformation> resultAddTags = await articleUseCase.AddTags(addTagsToArticle);
        if (resultAddTags.IsFailed)
        {
            throw new GraphQLException(
                ErrorBuilder
                .New()
                .SetMessage(string.Join(Environment.NewLine, resultAddTags.Errors.Select(pre => pre.Message)))
                .Build());
        }
        return resultAddTags.Value;
    }
    public async ValueTask<ArticleInformation> RemoveTags(IArticleUseCase articleUseCase,
        RemoveTagsToArticle removeTagsToArticle)
    {
        FluentResults.Result<ArticleInformation> resultRemoveTags = await articleUseCase.RemoveTags(removeTagsToArticle);
        if (resultRemoveTags.IsFailed)
        {
            throw new GraphQLException(
                ErrorBuilder
                .New()
                .SetMessage(string.Join(Environment.NewLine, resultRemoveTags.Errors.Select(pre => pre.Message)))
                .Build());
        }
        return resultRemoveTags.Value;
    }
    
}

public class MutationType : ObjectType<Mutation>
{
    protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor.Authorize();

        descriptor
          .Field(f => f.UpdateMyUser(default!, default!, default!))
          .UseTransactionMiddleware()
          .Type<UserInformationType>();

        descriptor
          .Field(f => f.CreateArticle(default!, default!, default!))
          .UseTransactionMiddleware()
          .Type<ArticleInformationType>();

        descriptor
          .Field(f => f.CreateTags(default!, default!, default!))
          .UseTransactionMiddleware()
          .UsePaging()
          .Type<ListType<TagInformationType>>();

        descriptor
            .Field(f => f.AddTags(default!,default!))
            .UseTransactionMiddleware()
            .Type<ArticleInformationType>();
    }
}
