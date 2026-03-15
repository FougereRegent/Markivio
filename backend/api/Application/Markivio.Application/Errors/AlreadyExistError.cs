using FluentResults;

namespace Markivio.Application.Errors;

public class AlreadyExistError : Error
{
    public AlreadyExistError(string message) : base(message)
    {
		Metadata.Add(ErrorCode.ERROR_CODE_PROPERTY_NAME, ErrorCode.ITEM_ALREADY_EXIST);
    }
}
