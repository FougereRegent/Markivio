using FluentResults;

namespace Markivio.Application.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string message) : base(message) { 
      Metadata.Add(ErrorCode.ERROR_CODE_PROPERTY_NAME, ErrorCode.ITEM_NOT_FOUND);
    }
}

