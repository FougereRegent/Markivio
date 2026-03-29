using Markivio.Presentation.Exceptions;

namespace Markivio.Presentation.Extensions;

internal static class FluentResultsExtention
{

    extension(FluentResults.Result source)
    {
        internal void ThrowIfResultIsFailed()
        {
            if (source.IsFailed)
                throw new ApplicationLayerException(source.Errors);
        }
    }

    extension<T>(FluentResults.Result<T> source)
    {
        internal T ThrowIfResultIsFailed()
        {
            if (source.IsFailed)
                throw new ApplicationLayerException(source.Errors);
            return source.Value;
        }
    }

}
