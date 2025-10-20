using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;

namespace Markivio.Presentation.GraphQl;

public class Query
{
    public User GetUser(IUserRepository userRepository)
    {
        return userRepository.GetAll().First();
    }
}

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
          .Field(f => f.GetUser(default!))
          .Type<UserType>();
    }
}
