using FluentResults;

namespace Markivio.Application.Errors;

public class AlreadyExistError : Error
{
    public AlreadyExistError(string message) : base(message) { }
}
