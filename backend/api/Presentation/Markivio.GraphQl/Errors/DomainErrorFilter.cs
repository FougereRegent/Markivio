public class DomainErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
		if(error.Exception is Exception) {
		}
		return error;
    }
}
