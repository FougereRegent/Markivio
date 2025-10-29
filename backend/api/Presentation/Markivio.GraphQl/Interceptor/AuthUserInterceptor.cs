using HotChocolate.AspNetCore;
using Markivio.Application.Users;
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
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
    }
}
