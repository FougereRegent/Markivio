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
}

public class MutationType : ObjectType<Mutation>
{
    protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor.Authorize();

        descriptor
          .Field(f => f.UpdateMyUser(default!, default!, default!))
          .UseTransactionMildleware()
          .Type<UserInformationType>();

        descriptor
          .Field(f => f.CreateArticle(default!, default!, default!))
          .UseTransactionMildleware()
          .Type<ArticleInformationType>();

        descriptor
          .Field(f => f.CreateTags(default!, default!, default!))
          .UseTransactionMildleware()
          .UsePaging()
          .Type<ListType<TagInformationType>>();

    }

}
