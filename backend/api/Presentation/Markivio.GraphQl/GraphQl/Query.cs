using HotChocolate.Types.Pagination;
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

	public async ValueTask<TagInformation[]> SearchTags(ITagUseCase tagUseCase, string tagName, CancellationToken cancellationToken = default) {
		FluentResults.Result<TagInformation[]> result = await tagUseCase.SearchTagsByName(tagName, cancellationToken);

		if(result.IsFailed)
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
          .Type<UserInformationType>();

        descriptor
          .Field("me")
          .Resolve(context =>
          {
              IUserUseCase userUseCase = context.Service<IUserUseCase>();
              UserInformation result = userUseCase.CurrentUser;
              return result;
          })
          .Type<UserInformationType>();

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

        descriptor
          .Field("articles")
          .UseOffsetPaging(options: new PagingOptions()
          {
              MaxPageSize = 100,
              IncludeTotalCount = true,
              RequirePagingBoundaries = true,
              AllowBackwardPagination = false,
              DefaultPageSize = 50
          })
          .Argument("title", pre => pre.Type<StringType>())
          .Argument("tags", pre => pre.Type<ListType<StringType>>())
          .Resolve(context =>
          {
              string? title = context.ArgumentValue<string?>("title");
              List<string>? tags = context.ArgumentValue<List<string>?>("tags");
              IArticleUseCase articleUseCase = context.Service<IArticleUseCase>();

              return articleUseCase.FindByFilter(new ArticleFilters(
                    title,
                    tags
                    ));
          });

        descriptor
          .Field("tags")
          .UseOffsetPaging(options: new PagingOptions()
          {
              MaxPageSize = 100,
              IncludeTotalCount = true,
              RequirePagingBoundaries = true,
              AllowBackwardPagination = false,
              DefaultPageSize = 50
          })
        .Resolve(context =>
        {
            ITagUseCase tagUseCase = context.Service<ITagUseCase>();
            return tagUseCase.GetAll();
        });

		descriptor
			.Field(f => f.SearchTags(default!,default!, default!))
			.Argument("tagName", pre => pre.Type<StringType>())
			.Type<ListType<TagInformationType>>();
    }
}
