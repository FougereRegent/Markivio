using Markivio.Domain.Entities;

namespace Markivio.UnitTests.Helper.Faker;

public sealed class ArticleContentGenerator : Bogus.Faker<ArticleContent>
{
    public ArticleContentGenerator()
    {
        CustomInstantiator(f =>
        {
            string source = f.Internet.Url();
            string content = f.Lorem.Slug();
            string description = f.Lorem.Sentence();
            return new ArticleContent(source: source, content: content, description: description);
        });
    }

    public static ArticleContent CreateValid()
    {
        ArticleContentGenerator contentGenerator = new ArticleContentGenerator();
        return contentGenerator.Generate();
    }
}
