using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper]
public partial class UserMapper
{
    public partial UserInformation UserToUserInformation(User user);
}

[Mapper]
public static partial class UserMapperProjection
{
    public static partial IQueryable<UserInformation> ProjectionToDto(this IQueryable<User> users);
}
