namespace Markivio.UnitTests.Helper.Faker;
using Bogus;
using Markivio.Domain.ValueObject;

public sealed class TagValueObjectFaker : Faker<TagValueObject>
{
    public TagValueObjectFaker()
    {
        CustomInstantiator(f =>
        {
            var name = GenerateValidName(f);
            var color = GenerateValidColor(f);

            return new TagValueObject(name, color);
        });
    }

    private static string GenerateValidName(Faker f)
    {
        var raw = f.Commerce.ProductName();

        // Nettoyage pour matcher ton regex
        var cleaned = new string(raw
            .Where(c => char.IsLetterOrDigit(c) || " -_'&".Contains(c))
            .ToArray());

        if (string.IsNullOrWhiteSpace(cleaned))
            cleaned = f.Random.String2(3, 10, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");

        return cleaned.Length > 25
            ? cleaned.Substring(0, 25)
            : cleaned;
    }

    private static string GenerateValidColor(Faker f)
    {
        // Génère exactement #RRGGBB
        return f.Random.Hexadecimal(6, prefix: "#");
    }
}
