using FluentResults;

namespace Markivio.Domain.Errors;

public sealed class ExceedElementsError : Error
{
    public ExceedElementsError(string message) : base(message)
    {
    }

    public ExceedElementsError(int nombreMaxElement) : base()
    {
        Message = $"{nombreMaxElement} ";
    }

    public ExceedElementsError(int nombreMaxElement, string propertyName) : base()
    {
        Message = $"{nombreMaxElement} {propertyName}";
    }
}
