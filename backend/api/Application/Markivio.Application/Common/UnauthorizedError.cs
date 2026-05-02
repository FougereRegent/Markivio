using FluentResults;

namespace Markivio.Application.Errors;

public class UnauthorizedError : Error
{
    public UnauthorizedError(string message) : base(message)
    {
        Metadata.Add("StatusCode", 403);
    }
}
