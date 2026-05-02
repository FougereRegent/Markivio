using Markivio.Presentation.Middleware;
namespace Markivio.Presentation.GraphQl.Article;

public static class AddTags
{
    extension(IObjectTypeDescriptor<Mutation> descriptor)
    {
        public IObjectTypeDescriptor<Mutation> MapAddTags()
        {
            descriptor
                .Field(f => f.AddTags(default!, default!))
                .UseTransactionMiddleware()
                .Type<ArticleInformationType>();
            return descriptor;
        }
    }
}
