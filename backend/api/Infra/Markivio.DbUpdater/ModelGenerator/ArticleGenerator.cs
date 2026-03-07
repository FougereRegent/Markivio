using Bogus;
using Markivio.Domain.Entities;

namespace Markivio.DbUpdater.ModelGenerator;

public class ArticleGenerator : Faker<Article>
{
    private ArticleContentGenerator contentGenerator = new ArticleContentGenerator();
    public ArticleGenerator()
    {
        RuleFor(o => o.Title, f => f.Hacker.Noun());
        RuleFor(o => o.ArticleContent, _ => contentGenerator.Generate());
    }
}

public class ArticleContentGenerator : Faker<ArticleContent>
{
    public ArticleContentGenerator()
    {
        RuleFor(o => o.Source, f => f.Internet.Url());
        RuleFor(o => o.Content, f => f.Lorem.Paragraph());
    }
}
