using Markivio.Application.Dto;
using Markivio.Application.UseCases;

public partial class Query {
    public async Task<UserInformation> GetUserById(IUserUseCase userUseCase, Guid id, CancellationToken cancellationToken = default)
    {
        FluentResults.Result<UserInformation> result = await userUseCase.GetUserInformationById(id, cancellationToken);
        if (result.IsFailed)
            throw new InvalidOperationException();

        return result.Value;
    }
}
