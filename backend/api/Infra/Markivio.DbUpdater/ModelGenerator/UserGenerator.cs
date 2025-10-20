using Bogus;
using Markivio.Domain.Entities;

namespace Markivio.DbUpdater.ModelGenerator;

public class UserGenerator : Faker<User>
{
    public UserGenerator()
    {
        RuleFor(o => o.Email, f => f.Internet.Email());
        RuleFor(o => o.FirstName, f => f.Person.FirstName);
        RuleFor(o => o.LastName, f => f.Person.LastName);
    }
}
