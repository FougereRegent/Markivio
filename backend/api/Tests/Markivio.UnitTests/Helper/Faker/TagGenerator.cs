using Markivio.Domain.Entities;
using Markivio.Domain.ValueObject;

namespace Markivio.UnitTests.Helper.Faker;

public static class TagValueGenerator {
    public static  Tag CreateValidTag(string? name = null, string? color = null)
    {
		Bogus.Faker faker = new Bogus.Faker();
        string safeName = name ?? faker.Random.String2(10, "abcdefghijklmnopqrstuvwxyz");
        string safeColor = color ?? ("#" + faker.Random.String2(6, "0123456789ABCDEF"));

		return new Tag(new TagValueObject(name: safeName, color: safeColor));
    }
}
