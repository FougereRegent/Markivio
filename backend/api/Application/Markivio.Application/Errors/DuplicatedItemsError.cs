using FluentResults;

namespace Markivio.Application.Errors;

public class DuplicatedItemsError : Error
{
    public DuplicatedItemsError() : base("You have duplicated items in your collections")
    {
        Metadata.Add(ErrorCode.ERROR_CODE_PROPERTY_NAME, ErrorCode.DUPLIACTED_ITEM);
    }
}

