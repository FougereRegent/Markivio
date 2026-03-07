using HotChocolate.Types.Pagination;
using Markivio.Application.Dto;
using Markivio.Application.UseCases;


namespace Markivio.Presentation.GraphQl;

public class Query
{
	public UserInformation Me(IUserUseCase userUseCase) {
		return userUseCase.CurrentUser;
	}

	public IQueryable<ArticleInformation> Articles(IArticleUseCase articleUseCase, string? title, List<string>? tags) {
	  return articleUseCase.FindByFilter(new ArticleFilters(
			title,
			tags
			));
	}

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
          .Type<UserInformationType>();

        descriptor
          .Field(f => f.Me(default!))
          .Type<UserInformationType>();

        descriptor
          .Field(f => f.Articles(default!, default!, default!))
          .UseOffsetPaging(options: new PagingOptions()
          {
              MaxPageSize = 100,
              IncludeTotalCount = true,
              RequirePagingBoundaries = true,
              AllowBackwardPagination = false,
              DefaultPageSize = 50
          });

        descriptor
          .Field("tags")
		  .Argument("tagName", f => f.Type<StringType?>())
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
			string tagName = context.ArgumentValue<string?>("tagName") ?? "";
            ITagUseCase tagUseCase = context.Service<ITagUseCase>();
            return tagUseCase.GetAllTags(tagName);
        });
    }
}
