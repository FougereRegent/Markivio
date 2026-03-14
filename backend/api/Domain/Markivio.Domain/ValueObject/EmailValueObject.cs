using Markivio.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Markivio.Domain.ValueObject;

public sealed class EmailValueObject : BaseValueObject
{
    private const string REGEX_EMAIL = @"^[\w\-\.]+@([\w-]+\.)+[\w-]{2,}$";
	public string Email {get; init;}

    public EmailValueObject(string email)
    {
		if(string.IsNullOrEmpty(email))
			throw new EmptyException($"email cannot be empty", "EMPTY_EMAIL");

		if(!Regex.IsMatch(email, REGEX_EMAIL))
			throw new PatternException($"email : {email} doesn't match", "FORMAT_EMAIL");

		Email = email;

    }

    protected override IEnumerable<object> GetAtomicValues()
    {
		yield return Email;
    }
}
