using HotChocolate.Types.Pagination;
using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;

namespace Markivio.Presentation.GraphQl;

public class Query
{
    public ValueTask<User?> GetUserById(IUserRepository userRepository, Guid id)
    {
        return userRepository.GetById(id);
    }
}

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
          .Field(f => f.GetUserById(default!, default!))
          .Argument("id", args => args.Type<UuidType>())
          .Type<UserType>();

        descriptor
          .Field("users")
          .UseOffsetPaging(options: new PagingOptions()
          {
              MaxPageSize = 100,
              IncludeTotalCount = true,
              RequirePagingBoundaries = true,
              AllowBackwardPagination = false
          })
          .Resolve(context =>
          {
              IUserRepository userRepository = context.Service<IUserRepository>();
              return userRepository.GetAll();
          });
    }
}
