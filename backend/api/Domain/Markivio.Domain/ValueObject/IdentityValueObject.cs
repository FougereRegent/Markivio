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
		CheckFirstName(firstName);
		CheckLastName(lastName);

		Username = userName;
		FirstName = firstName;
		LastName = lastName;
    }

	public void UpdateFirstName(string firstName) {
		CheckFirstName(firstName);
		FirstName = firstName;
	}

	public void UpdateLastName(string lastName) {
		CheckLastName(lastName);
		LastName = lastName;
	}

    protected override IEnumerable<object> GetAtomicValues()
    {
		yield return Username;
		yield return FirstName;
		yield return LastName;
    }

	private static void CheckFirstName(string firstName) {
		if(string.IsNullOrEmpty(firstName))
			throw new EmptyException("firstname cannot be empty", "EMPTY_FIRSTNAME");
		if(!Regex.IsMatch(firstName, REGEX_FIRSTNAME_AND_LASTNAME))
			throw new PatternException("regex not match", "FORMAT_FIRSTNAME");
	}

	private static void CheckLastName(string lastName) {
		if(string.IsNullOrEmpty(lastName))
			throw new EmptyException("lastname cannot be empty", "EMPTY_LASTNAME");

		if(!Regex.IsMatch(lastName, REGEX_FIRSTNAME_AND_LASTNAME))
			throw new PatternException("regex not match", "FORMAT_LASTNAME");

	}
}
