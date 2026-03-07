namespace Markivio.Application.Errors;

using FluentResults;

public class DuplicatedItemsError : Error
{
    public DuplicatedItemsError() : base("You have duplicated items in your collections")
    {
        Metadata.Add("StatusCode", 400);
    }
}

