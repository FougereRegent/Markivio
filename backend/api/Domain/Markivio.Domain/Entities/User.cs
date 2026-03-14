using Markivio.Domain.ValueObject;

namespace Markivio.Domain.Entities;


public sealed class User : Entity
{
    private const string REGEX_FIRSTNAME_AND_LASTNAME = @"^[A-Za-zÀ-ÿà-ÿ\-\'’]+(?:\s[\.\'’\,A-Za-zÀ-ÿà-ÿ\-]+)*$";

    public EmailValueObject Email { get; set; } = null!;
	public IdentityValueObject Identity {get;set;} = null!;
    public string AuthId { get; set; } = string.Empty;

	public User(IdentityValueObject identityValue, EmailValueObject emailValue) {
		Email = emailValue;
		Identity = identityValue;
	}
}
