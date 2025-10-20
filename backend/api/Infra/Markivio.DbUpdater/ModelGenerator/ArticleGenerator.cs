using Bogus;
using Markivio.Domain.Entities;

namespace Markivio.DbUpdater.ModelGenerator;

public class ArticleGenerator : Faker<Article>
{
    public ArticleGenerator()
    {
        RuleFor(o => o.Source, f => f.Internet.Url());
        RuleFor(o => o.Title, f => f.Hacker.Noun());
        RuleFor(o => o.Content, f => f.Lorem.Paragraph());
    }
}
