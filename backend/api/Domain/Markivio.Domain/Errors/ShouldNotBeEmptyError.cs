using FluentResults;

namespace Markivio.Domain.Errors;

public class ShouldNotBeEmptyError : Error
{
    public ShouldNotBeEmptyError(string propertyName) : base($"{propertyName} should not be empty") { }
}
