using Markivio.Domain.Entities;
using Markivio.Domain.ValueObject;
using Shouldly;

namespace Markivio.UnitTests.Domain.Entities;

public sealed class TagEntityTests : BaseTests
{
    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenTagValueIsNull()
    {
        // Arrange
        TagValueObject tagValue = null!;

        // Act
        var act = () => new Tag(tagValue);

        // Assert
        Should.Throw<ArgumentNullException>(act).ParamName.ShouldBe("tagValue");
    }

    [Fact]
    public void Ctor_ShouldCreate_WhenTagValueIsProvided()
    {
        // Arrange
        var tagValue = new TagValueObject(
            name: faker.Random.String2(10, "abcdefghijklmnopqrstuvwxyz"),
            color: "#" + faker.Random.String2(6, "0123456789ABCDEF"));

        // Act
        var tag = new Tag(tagValue);

        // Assert
        tag.TagValue.ShouldBe(tagValue);
    }
}

