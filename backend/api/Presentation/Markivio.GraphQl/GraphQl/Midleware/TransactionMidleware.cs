using Markivio.Persistence;

namespace Markivio.Presentation.GraphQl.Midleware;

internal static class TransactionMidleware
{
    internal static IObjectFieldDescriptor UseTransactionMildleware(this IObjectFieldDescriptor descriptor)
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
