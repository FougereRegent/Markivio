using HotChocolate.Types.Pagination;
using HotChocolate.Resolvers;
using Markivio.Application.Dto;
using Markivio.Application.Users;


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

        descriptor
          .Authorize();
        descriptor
          .Field(f => f.GetUserById(default!, default!))
          .Argument("id", args => args.Type<UuidType>())
          .Type<UserType>();

        descriptor
          .Field("me")
          .Resolve(async context =>
          {
              IUserUseCase userUseCase = context.Service<IUserUseCase>();
              string token = context.GetGlobalState<string>("token");
              Console.WriteLine(token);

              FluentResults.Result<UserInformation> result = await userUseCase.Me(new UserConnectionDto(token));
              if (result.IsSuccess)
                  return result.Value;
              return new UserInformation();
          })
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
