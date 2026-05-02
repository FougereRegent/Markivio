using Markivio.Application.Dto;
using Markivio.Application.UseCases;

namespace Markivio.Presentation.GraphQl;

public partial class Query
{

    public UserInformation Me(IUserUseCase userUseCase)
    {
        return userUseCase.CurrentUser;
    }

    public async Task<UserInformation> GetUserById(IUserUseCase userUseCase, Guid id, CancellationToken cancellationToken = default)
    {
        FluentResults.Result<UserInformation> result = await userUseCase.GetUserInformationById(id, cancellationToken);
        if (result.IsFailed)
            throw new InvalidOperationException();

        return result.Value;
    }
}
