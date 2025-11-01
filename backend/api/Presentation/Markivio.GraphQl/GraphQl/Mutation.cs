using Markivio.Application.Dto;
using Markivio.Application.Users;
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
}

public class MutationType : ObjectType<Mutation>
{
    protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor.Authorize();

        descriptor
          .Field(f => f.UpdateMyUser(default!, default!, default!))
          .UseTransactionMildleware()
          .Type<UserType>();
    }

}
