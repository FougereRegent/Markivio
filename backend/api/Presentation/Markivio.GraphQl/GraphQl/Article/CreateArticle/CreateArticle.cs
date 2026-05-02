using Markivio.Presentation.Middleware;

namespace Markivio.Presentation.GraphQl.Article;

public static class CreateArticle
{
    extension(IObjectTypeDescriptor<Mutation> descriptor)
    {
        public IObjectTypeDescriptor<Mutation> MapCreateArticle()
        {
            descriptor
              .Field(f => f.CreateArticle(default!, default!, default!))
              .UseTransactionMiddleware()
              .Type<ArticleInformationType>();
            return descriptor;
        }
    }
}
