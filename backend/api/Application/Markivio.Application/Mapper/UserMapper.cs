using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Markivio.Domain.ValueObject;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper]
public partial class UserMapper
{
	[MapNestedProperties(nameof(User.Identity))]
	[MapNestedProperties(nameof(User.Email))]
    public partial UserInformation UserToUserInformation(User user);

	public void ApplyUpdate(UpdateUserInformation update, User user)
	{
		ArgumentNullException.ThrowIfNull(user);
		ArgumentNullException.ThrowIfNull(update);

		// Preserve username, rebuild the VO to re-run domain validation.
		user.Identity = new IdentityValueObject(
			user.Identity.Username,
			update.FirstName,
			update.LastName);
	}
}
