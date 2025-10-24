using HotChocolate.Types.Pagination;
using Markivio.Application.Dto;
using Markivio.Application.Users;
using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using FluentResults;

namespace Markivio.Presentation.GraphQl;

public class Query
{
    public async ValueTask<UserInformation> GetUserById(IUserUseCase userUseCase, Guid id)
    {
        FluentResults.Result<UserInformation> result = await userUseCase.GetUserInformationById(id);
        if (result.IsFailed)
            throw new InvalidOperationException();

        return result.Value;
    }
}

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {

        descriptor.Authorize();
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
              IUserUseCase userUseCase = context.Service<IUserUseCase>();
              return userUseCase.GetUsers();
          });
    }
}
