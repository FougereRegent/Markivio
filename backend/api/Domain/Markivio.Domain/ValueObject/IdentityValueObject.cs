using System.Text.RegularExpressions;
using Markivio.Domain.Exceptions;

namespace Markivio.Domain.ValueObject;

public sealed class IdentityValueObject : BaseValueObject
{
    private const string REGEX_FIRSTNAME_AND_LASTNAME = @"^[A-Za-zÀ-ÿà-ÿ\-\'’]+(?:\s[\.\'’\,A-Za-zÀ-ÿà-ÿ\-]+)*$";

	public string Username {get; set;} = string.Empty;
	public string LastName {get; set;} = string.Empty;
	public string FirstName {get; set;} = string.Empty;

	private IdentityValueObject() {}

    public IdentityValueObject(string userName, string firstName, string lastName)
    {
		if(string.IsNullOrEmpty(firstName))
			throw new EmptyException("firstname cannot be empty", "EMPTY_FIRSTNAME");

		if(string.IsNullOrEmpty(lastName))
			throw new EmptyException("lastname cannot be empty", "EMPTY_LASTNAME");

		if(!Regex.IsMatch(firstName, REGEX_FIRSTNAME_AND_LASTNAME))
			throw new PatternException("regex not match", "FORMAT_FIRSTNAME");

		if(!Regex.IsMatch(lastName, REGEX_FIRSTNAME_AND_LASTNAME))
			throw new PatternException("regex not match", "FORMAT_LASTNAME");

		Username = userName;
		FirstName = firstName;
		LastName = lastName;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
		yield return Username;
		yield return FirstName;
		yield return LastName;
    }
}
