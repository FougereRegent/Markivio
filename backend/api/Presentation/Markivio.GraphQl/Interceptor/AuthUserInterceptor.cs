
using HotChocolate.AspNetCore;
using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;

namespace Markivio.Presentation.Interceptor;

public class AuthUserInterceptor : DefaultHttpRequestInterceptor
{
    public override async ValueTask OnCreateAsync(HttpContext context,
        HotChocolate.Execution.IRequestExecutor requestExecutor,
        HotChocolate.Execution.OperationRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        IUserRepository userRepository = context.RequestServices.GetRequiredService<IUserRepository>();
        string authHeader = context.Request.Headers.Authorization.FirstOrDefault(string.Empty)!;
        if (!string.IsNullOrEmpty(authHeader) && authHeader.Contains("Bearer"))
        {
            string token = authHeader.Substring("Bearer ".Length);
            User user = await userRepository.GetUserInfoByToken(token, cancellationToken)!;
            Console.WriteLine(user.Email);
        }
        await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
    }
}
