using Markivio.Domain.Entities;

namespace Markivio.Presentation.GraphQl;

public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {

    }
}
