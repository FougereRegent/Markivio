namespace Markivio.Presentation.GraphQl.Article;

public static class ArticleMutation
{
    extension(IObjectTypeDescriptor<Mutation> descriptor)
    {
		public IObjectTypeDescriptor<Mutation> MapArticleMutation() {
			descriptor.MapCreateArticle()
				.MapUpdateArticle()
				.MapAddTags();
			return descriptor;
		}
    }
}
