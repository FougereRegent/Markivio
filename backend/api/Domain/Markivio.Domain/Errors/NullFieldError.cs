using FluentResults;

namespace Markivio.Domain.Errors;

public sealed class NullFieldError : Error
{
    public NullFieldError(string propertyName) : base($"{propertyName} cannot be null") { }
}
