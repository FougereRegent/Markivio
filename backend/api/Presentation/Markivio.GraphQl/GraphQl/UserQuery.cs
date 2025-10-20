using Markivio.Domain.Entities;

namespace Markivio.Presentation.GraphQl;

public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor
          .Field(f => f.FirstName)
          .Type<StringType>();
        descriptor
          .Field(f => f.LastName)
          .Type<StringType>();
    }
}

