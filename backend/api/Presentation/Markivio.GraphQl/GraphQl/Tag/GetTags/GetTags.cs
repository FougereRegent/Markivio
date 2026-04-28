using HotChocolate.Types.Pagination;
using Markivio.Application.UseCases;

namespace Markivio.Presentation.GraphQl.Tag;

public static class GetTags
{
    extension(IObjectTypeDescriptor<Query> descriptor)
    {
        public IObjectTypeDescriptor<Query> MapGetTags()
        {
            descriptor
                .Field("tags")
                .Argument("tagName", f => f.Type<StringType?>())
                .UseOffsetPaging(options: new PagingOptions()
                {
                    MaxPageSize = 100,
                    IncludeTotalCount = true,
                    RequirePagingBoundaries = false,
                    AllowBackwardPagination = false,
                    DefaultPageSize = 50,
                })
                .Resolve(context =>
                {
                    string tagName = context.ArgumentValue<string?>("tagName") ?? string.Empty;
                    var tagUseCase = context.Services.GetRequiredService<ITagUseCase>();
                    return tagUseCase.GetAllTags(tagName);
                });
            return descriptor;
        }
    }
}
