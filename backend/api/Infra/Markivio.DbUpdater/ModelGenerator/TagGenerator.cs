using Bogus;
using Markivio.Domain.Entities;

namespace Markivio.DbUpdater.ModelGenerator;

public class TagGenerator : Faker<Tag>
{
    public TagGenerator()
    {
        RuleFor(o => o.Name, f => f.Hacker.Noun());
    }
}
