using Markivio.Presentation.Exceptions;
using System.Diagnostics;

namespace Markivio.Presentation.ErrorFilters;

public class DomainErrorFilter : IErrorFilter
{
	public readonly ILogger<DomainErrorFilter> _logger;

	public DomainErrorFilter(ILogger<DomainErrorFilter> logger) {
		_logger = logger;
	}

    public IError OnError(IError error)
    {
		string traceId = Activity.Current?.TraceId.ToString() ?? string.Empty;
		if(error.Exception is ApplicationLayerException exp) {
			_logger.LogWarning("Domain error code : {error}, message : {message}, traceId : {traceId}",exp.ErrorCode, exp.Message, traceId);
			return (new ErrorBuilder())
				.SetCode(exp.ErrorCode)
				.SetMessage(exp.Message)
				.Build();
		}
		if(error.Exception is not null) {
			_logger.LogError("Unandle error : {error}, traceId : {traceId}", error.Exception.Message, traceId);
		}
		return error;
    }
}
