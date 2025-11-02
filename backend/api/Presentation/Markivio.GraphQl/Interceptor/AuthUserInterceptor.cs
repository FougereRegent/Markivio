using HotChocolate.AspNetCore;
using Markivio.Application.UseCases;
using Markivio.Application.Dto;
using FluentResults;
using Markivio.Persistence;

namespace Markivio.Presentation.Interceptor;

public class AuthUserInterceptor : DefaultHttpRequestInterceptor
{
    public override async ValueTask OnCreateAsync(HttpContext context,
        HotChocolate.Execution.IRequestExecutor requestExecutor,
        HotChocolate.Execution.OperationRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        IUserUseCase userUseCase = context.RequestServices.GetRequiredService<IUserUseCase>();
        IUnitOfWork unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>();
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        string authHeader = context.Request.Headers.Authorization.FirstOrDefault(string.Empty)!;

        if (!string.IsNullOrEmpty(authHeader) && authHeader.Contains("Bearer"))
        {
            string token = authHeader.Substring("Bearer ".Length);
            requestBuilder.SetGlobalState("token", token);
            Result result = await userUseCase.CreateNewUserOnConnection(new UserConnectionDto(token), cancellationToken)!;
            if (result.IsFailed)
            {
                await unitOfWork.RollbackChangesAsync(cancellationToken);
                throw new GraphQLException(ErrorBuilder.New()
                    .SetMessage(string.Join(Environment.NewLine, result.Errors.Select(pre => pre.Message)))
                    .Build());
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await SetCurrentUser(userUseCase, token, cancellationToken);
        }
        await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
    }

    private static async Task SetCurrentUser(IUserUseCase userUseCase, string token, CancellationToken cancellationToken = default)
    {
        FluentResults.Result<UserInformation> userInformation = await userUseCase.Me(new UserConnectionDto(token));
        if (userInformation.IsFailed)
            throw new GraphQLException(ErrorBuilder.New()
                .SetMessage(
                  string.Join(Environment.NewLine, userInformation.Errors.Select(pre => pre.Message)))
                .Build());

        userUseCase.CurrentUser = userInformation.Value;
    }
}
