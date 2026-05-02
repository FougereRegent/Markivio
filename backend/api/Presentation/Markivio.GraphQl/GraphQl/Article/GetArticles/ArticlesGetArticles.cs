using HotChocolate.Types.Pagination;
namespace Markivio.Presentation.GraphQl.Article;

public static class GetArticles
{
    extension(IObjectTypeDescriptor<Query> descriptor)
    {
        public IObjectTypeDescriptor<Query> MapArticles()
        {
            descriptor
              .Field(f => f.Articles(default!, default!, default!))
              .UseOffsetPaging(options: new PagingOptions()
              {
                  MaxPageSize = 100,
                  IncludeTotalCount = true,
                  RequirePagingBoundaries = true,
                  AllowBackwardPagination = false,
                  DefaultPageSize = 50,
              })
            .UseFiltering()
            .UseSorting();
            return descriptor;
        }
    }
}
