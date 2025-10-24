using Markivio.Application.Dto;
using Markivio.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Markivio.Application.Mapper;

[Mapper]
public partial class UserMapper
{
    public partial UserInformation UserToUserInformation(User user);
}
