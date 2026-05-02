namespace Markivio.Presentation.GraphQl.User;

public static class GetUserByIdQuery
{
    extension(IObjectTypeDescriptor<Query> descriptor)
    {
        public IObjectTypeDescriptor<Query> MapGetUserById()
        {
            descriptor
              .Field(f => f.GetUserById(default!, default!, default!))
              .Argument("id", args => args.Type<UuidType>())
              .Type<UserInformationType>();
            return descriptor;
        }
    }
}
