using Markivio.Application.Dto;

namespace Markivio.Presentation.GraphQl.User;

public sealed class UserInformationType : ObjectType<UserInformation>
{
    protected override void Configure(IObjectTypeDescriptor<UserInformation> descriptor)
    {
        descriptor
          .Field(f => f.Id)
          .Type<UuidType>();

        descriptor
          .Field(f => f.FirstName)
          .Type<StringType>();

        descriptor
          .Field(f => f.LastName)
          .Type<StringType>();

        descriptor
          .Field(f => f.Email)
          .Type<StringType>();
    }
}
