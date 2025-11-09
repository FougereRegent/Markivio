using FluentResults;

namespace Markivio.Domain.Errors;

public class FormatUnexpectedError : Error
{
    public FormatUnexpectedError(string message) : base(message) { }

    public FormatUnexpectedError(string propertyName, string format = "") : base()
    {
        if (string.IsNullOrEmpty(format))
            Message = $"{propertyName} is in bad format";
        else
            Message = $"{propertyName} is in bad format : format";
    }
}
