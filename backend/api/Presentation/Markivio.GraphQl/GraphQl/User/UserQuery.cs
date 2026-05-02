namespace Markivio.Presentation.GraphQl.User;


public static class UserQuery
{
    extension(IObjectTypeDescriptor<Query> descriptor)
    {
        public IObjectTypeDescriptor<Query> MapUserQuery()
        {
            descriptor.MapUserMeQuery();
            descriptor.MapGetUserById();
            return descriptor;
        }
    }
}
