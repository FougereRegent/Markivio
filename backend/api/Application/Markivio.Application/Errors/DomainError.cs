using FluentResults;
using Markivio.Domain.Exceptions;

namespace Markivio.Application.Errors;

public sealed class DomainError : Error {
	private DomainError(DomainException ex) {
		base.Message = ex.Message;
		base.Metadata.Add(ErrorCode.ERROR_CODE_PROPERTY_NAME, ex.ErrorCode);
	}

	public static DomainError Create(DomainException ex) {
		return new DomainError(ex);
	}
}
