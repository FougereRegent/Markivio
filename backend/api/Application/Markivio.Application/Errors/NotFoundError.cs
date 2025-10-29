namespace Markivio.Application.Errors;

using System.Collections.Generic;
using FluentResults;

public class NotFoundError : Error
{
    public NotFoundError(string message) : base(message)
    {
        Metadata.Add("StatusCode", 404);
    }
}

