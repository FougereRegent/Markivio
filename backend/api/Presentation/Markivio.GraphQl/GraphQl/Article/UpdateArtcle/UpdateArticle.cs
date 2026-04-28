using Markivio.Presentation.Middleware;
namespace Markivio.Presentation.GraphQl.Article;

public static class UpdateArticle
{
    extension(IObjectTypeDescriptor<Mutation> descriptor)
    {
        public IObjectTypeDescriptor<Mutation> MapUpdateArticle()
        {
            descriptor
                .Field(f => f.UpdateArticle(default!, default!, default!))
                .UseTransactionMiddleware()
                .Type<ArticleInformationType>();
            return descriptor;
        }

    }
}
