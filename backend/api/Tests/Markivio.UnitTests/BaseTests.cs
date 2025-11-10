using Bogus;

namespace Markivio.UnitTests;

public abstract class BaseTests
{
    protected readonly Faker faker = new Faker("fr");
}
