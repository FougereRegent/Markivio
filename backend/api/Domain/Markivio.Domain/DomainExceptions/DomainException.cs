namespace Markivio.Domain.Exceptions;

public abstract class DomainException(string message) : Exception(message) {
	public abstract string ErrorCode { get; protected set; }
}
