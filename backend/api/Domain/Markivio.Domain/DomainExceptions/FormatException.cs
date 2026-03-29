namespace Markivio.Domain.Exceptions;

public sealed class PatternException(string message, string errorCode) : DomainException(message)
{
    public override string ErrorCode { get; protected set; } = errorCode;
}
