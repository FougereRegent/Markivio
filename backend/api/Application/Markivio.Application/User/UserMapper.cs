#pragma warning disable RMG020
using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Markivio.Domain.ValueObject;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper]
public partial class UserMapper
{
    [MapProperty([nameof(User.Email), nameof(User.Email.Email)], nameof(UserInformation.Email))]
    [MapProperty([nameof(User.Identity), nameof(User.Identity.FirstName)], nameof(UserInformation.FirstName))]
    [MapProperty([nameof(User.Identity), nameof(User.Identity.LastName)], nameof(UserInformation.LastName))]
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

#pragma warning restore RMG020
