namespace Markivio.Presentation.GraphQl.Article;

public static class ArticleQuery
{
    extension(IObjectTypeDescriptor<Query> descriptor)
    {
        public IObjectTypeDescriptor<Query> MapArticleQuery()
        {
            descriptor.MapArticles();
            return descriptor;
        }
    }
}
