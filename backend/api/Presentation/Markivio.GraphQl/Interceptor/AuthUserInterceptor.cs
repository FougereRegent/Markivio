using HotChocolate.AspNetCore;
using Markivio.Application.UseCases;
using Markivio.Application.Dto;
using FluentResults;
using Markivio.Persistence;
using Markivio.Domain.Auth;
using Markivio.Domain.Repositories;
using Markivio.Extensions.Identity;
using Markivio.Domain.Entities;
namespace Markivio.Presentation.Interceptor;

public class AuthUserInterceptor : DefaultHttpRequestInterceptor
{
    public override async ValueTask OnCreateAsync(HttpContext context,
        HotChocolate.Execution.IRequestExecutor requestExecutor,
        HotChocolate.Execution.OperationRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        IAuthUser authUser = context.RequestServices.GetRequiredService<IAuthUser>();
        IUserUseCase userUseCase = context.RequestServices.GetRequiredService<IUserUseCase>();
        IUnitOfWork unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>();

        string authHeader = context.Request.Headers.Authorization.FirstOrDefault(string.Empty)!;

        if (!string.IsNullOrEmpty(authHeader) && authHeader.Contains("Bearer"))
        {
            string token = authHeader.Substring("Bearer ".Length);
            requestBuilder.SetGlobalState("token", token);

            User? user = await CheckIfUserExist(context, requestBuilder, token, cancellationToken);
            if (user is null)
            {
                await unitOfWork.BeginTransactionAsync(cancellationToken);

                Result result = await userUseCase.CreateNewUserOnConnection(new UserConnectionDto(token), cancellationToken)!;
                if (result.IsFailed)
                {
                    await unitOfWork.RollbackChangesAsync(cancellationToken);
                    throw new GraphQLException(ErrorBuilder.New()
                        .SetMessage(string.Join(Environment.NewLine, result.Errors.Select(pre => pre.Message)))
                        .Build());
                }
                await unitOfWork.SaveChangesAsync(cancellationToken);

                await SetCurrentUser(context, requestBuilder, token, cancellationToken);
            }
            else
            {
                requestBuilder.SetGlobalState("auth-user", user);
            }
        }
        await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
    }

    private static async Task SetCurrentUser(HttpContext context,
        HotChocolate.Execution.OperationRequestBuilder requestBuilder,
        string token,
        CancellationToken cancellationToken = default)
    {
        IUserRepository userRepository = context.RequestServices.GetRequiredService<IUserRepository>();
        JwtTokenInfo tokenInfo = JwtTokenExtentions.ParseToken(token);
        User? user = await userRepository.GetUserByAuthId(tokenInfo.Subject, cancellationToken);
        if (user is null)
            throw new GraphQLException(ErrorBuilder.New()
                .SetMessage(
                  "User not found")
                .Build());

        requestBuilder.SetGlobalState("auth-user", user);
    }

    private static async Task<User?> CheckIfUserExist(HttpContext context,
        HotChocolate.Execution.OperationRequestBuilder requestBuilder,
        string token,
        CancellationToken cancellationToken = default)
    {

        IUserRepository userRepository = context.RequestServices.GetRequiredService<IUserRepository>();
        JwtTokenInfo tokenInfo = JwtTokenExtentions.ParseToken(token);
        return await userRepository.GetUserByAuthId(tokenInfo.Subject, cancellationToken);
    }
}
