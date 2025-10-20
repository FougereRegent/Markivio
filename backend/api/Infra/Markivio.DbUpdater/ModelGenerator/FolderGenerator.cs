using Bogus;
using Markivio.Domain.Entities;

namespace Markivio.DbUpdater.ModelGenerator;

public class FolderGenerator : Faker<Folder>
{
    public FolderGenerator()
    {
        RuleFor(o => o.Name, f => f.Hacker.Noun());
    }
}
