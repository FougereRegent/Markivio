namespace Markivio.Presentation.GraphQl.User;

public static class UserMeQuery
{
    extension(IObjectTypeDescriptor<Query> descriptor)
    {
        public IObjectTypeDescriptor<Query> MapUserMeQuery()
        {
            descriptor
              .Field(f => f.Me(default!))
              .Type<UserInformationType>();
            return descriptor;
        }
    }
}

