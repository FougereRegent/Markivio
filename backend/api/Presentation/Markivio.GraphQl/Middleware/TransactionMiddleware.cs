using Markivio.Persistence;

namespace Markivio.Presentation.Middleware;

internal static class TransactionMiddleware
{
    internal static IObjectFieldDescriptor UseTransactionMiddleware(this IObjectFieldDescriptor descriptor)
    {
        return descriptor.Use(next => async context =>
        {
            IUnitOfWork unitOfWork = context.Service<IUnitOfWork>();
            await unitOfWork.BeginTransactionAsync(context.RequestAborted);

            try
            {
                await next(context);
            }
            catch
            {
                await unitOfWork.RollbackChangesAsync(context.RequestAborted);
                throw;
            }

            await unitOfWork.SaveChangesAsync(context.RequestAborted);
        });
    }
}
