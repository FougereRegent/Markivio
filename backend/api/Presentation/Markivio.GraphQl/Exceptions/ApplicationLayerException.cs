namespace Markivio.Presentation.Exceptions;

internal sealed class ApplicationLayerException : Exception
{
    internal string ErrorCode { get; set; } = string.Empty;

    public ApplicationLayerException(FluentResults.IError error)
    {
        KeyValuePair<string, object> val = error.Metadata.FirstOrDefault(pre => pre.Key == Markivio.Application.Errors.ErrorCode.ERROR_CODE_PROPERTY_NAME);
        ErrorCode = val.Value as string ?? string.Empty;
    }

    public ApplicationLayerException(IReadOnlyList<FluentResults.IError> errors)
    {
        if (!errors.Any())
        {
            ErrorCode = "UNKNOW_ERROR";
            return;
        }
        KeyValuePair<string, object> val = errors
            .First()
            .Metadata
            .FirstOrDefault(pre => pre.Key == Markivio.Application.Errors.ErrorCode.ERROR_CODE_PROPERTY_NAME);

        ErrorCode = val.Value as string ?? string.Empty;
    }
}
