using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Markivio.Domain.ValueObject;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper]
public partial class UserMapper
{
    [MapProperty([nameof(User.Email), nameof(User.Email.Email)], nameof(UserInformation.Email))]
    [MapNestedProperties(nameof(User.Identity))]
    public partial UserInformation UserToUserInformation(User user);

    public void ApplyUpdate(UpdateUserInformation update, User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        // Preserve username, rebuild the VO to re-run domain validation.
        user.Identity = new IdentityValueObject(
            user.Identity.Username,
            update.FirstName,
            update.LastName);
    }
}
