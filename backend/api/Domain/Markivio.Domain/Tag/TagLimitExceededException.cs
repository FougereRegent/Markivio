namespace Markivio.Domain.Exceptions;

public sealed class TagLimitExceededException(string message) : DomainException(message)
{
    public override string ErrorCode { get; protected set; } = "TAG_LIMIT_EXCEEDED";
}
