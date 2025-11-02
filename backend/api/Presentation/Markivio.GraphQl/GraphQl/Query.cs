using HotChocolate.Types.Pagination;
using HotChocolate.Resolvers;
using Markivio.Application.Dto;
using Markivio.Application.UseCases;


namespace Markivio.Presentation.GraphQl;

public class Query
{
    public async ValueTask<UserInformation> GetUserById(IUserUseCase userUseCase, Guid id, CancellationToken cancellationToken = default)
    {
        FluentResults.Result<UserInformation> result = await userUseCase.GetUserInformationById(id, cancellationToken);
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
          .Field(f => f.GetUserById(default!, default!, default!))
          .Argument("id", args => args.Type<UuidType>())
          .Type<UserType>();

        descriptor
          .Field("me")
          .Resolve(context =>
          {
              IUserUseCase userUseCase = context.Service<IUserUseCase>();
              UserInformation result = userUseCase.CurrentUser;
              return result;
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
