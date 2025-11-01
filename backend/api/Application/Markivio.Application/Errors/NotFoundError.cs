namespace Markivio.Application.Errors;

using FluentResults;

public class NotFoundError : Error
{
    public NotFoundError(string message) : base(message)
    {
        Metadata.Add("StatusCode", 404);
    }
}

