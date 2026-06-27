using HotChocolate.Types.Pagination;

namespace Markivio.Presentation.GraphQl;

public static class GraphqlOptions
{
    public readonly static PagingOptions OffsetPagingOptions = new PagingOptions
    {
        MaxPageSize = 100,
        IncludeTotalCount = true,
        RequirePagingBoundaries = false,
        AllowBackwardPagination = false,
        DefaultPageSize = 50,

    };
}
