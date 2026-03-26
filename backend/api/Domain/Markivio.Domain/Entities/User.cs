using Markivio.Domain.ValueObject;

namespace Markivio.Domain.Entities;


public sealed class User : Entity
{
    private const string REGEX_FIRSTNAME_AND_LASTNAME = @"^[A-Za-zÀ-ÿà-ÿ\-\'’]+(?:\s[\.\'’\,A-Za-zÀ-ÿà-ÿ\-]+)*$";

    public EmailValueObject Email { get; set; } = null!;
	public IdentityValueObject Identity {get;set;} = null!;
    public string AuthId { get; set; } = string.Empty;

	private User() {}

	public User(IdentityValueObject identityValue, EmailValueObject emailValue) {
		Identity = identityValue ?? throw new ArgumentNullException(nameof(identityValue));
		Email = emailValue ?? throw new ArgumentNullException(nameof(emailValue));
	}

	public void UpdateUser(string firstName, string lastName) {
		Identity.UpdateFirstName(firstName);
		Identity.UpdateLastName(lastName);
	}
}
