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
                .UseOffsetPaging(options: GraphqlOptions.OffsetPagingOptions)
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
