
using Markivio.Application.Dto;
using Markivio.Application.UseCases;

namespace Markivio.Presentation.GraphQl;

public partial class Mutation {
    public async Task<UserInformation> UpdateMyUser(IUserUseCase userUseCase,
        UpdateUserInformation updateUserInformation,
        CancellationToken cancellationToken = default)
    {
        FluentResults.Result<UserInformation> resultUpdate = await userUseCase.UpdateCurrentUser(updateUserInformation);
        if (resultUpdate.IsFailed)
        {
            throw new GraphQLException(ErrorBuilder
                .New()
                .SetCode(resultUpdate.Errors[0].GetType().Name)
                .SetMessage(string.Join(Environment.NewLine, resultUpdate.Errors.Select(pre => pre.Message)))
                .Build()
                );
        }
        return resultUpdate.Value;
    }

}
