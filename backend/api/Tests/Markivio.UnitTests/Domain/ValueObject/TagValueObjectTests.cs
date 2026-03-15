using Markivio.Domain.Exceptions;
using Markivio.Domain.ValueObject;
using Shouldly;

namespace Markivio.UnitTests.Domain.ValueObject;

public sealed class TagValueObjectTests : BaseTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Ctor_ShouldThrowEmptyException_WhenNameIsNullOrEmpty(string? name)
    {
        // Arrange
        string color = "#FFFFFF";

        // Act
        var act = () => new TagValueObject(name!, color);

        // Assert
        EmptyException ex = Should.Throw<EmptyException>(act);
        ex.ErrorCode.ShouldBe("EMPTY_TAGNAME");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Ctor_ShouldThrowEmptyException_WhenColorIsNullOrEmpty(string? color)
    {
        // Arrange
        string name = faker.Random.String2(10, "abcdefghijklmnopqrstuvwxyz");

        // Act
        var act = () => new TagValueObject(name, color!);

        // Assert
        EmptyException ex = Should.Throw<EmptyException>(act);
        ex.ErrorCode.ShouldBe("EMPTY_COLORTAG");
    }

    [Theory]
    [InlineData("!!::")]
    [InlineData("test!")]
    [InlineData("???")]
    public void Ctor_ShouldThrowPatternException_WhenNameDoesNotMatchRegex(string name)
    {
        // Arrange
        string color = "#FFFFFF";

        // Act
        var act = () => new TagValueObject(name, color);

        // Assert
        PatternException ex = Should.Throw<PatternException>(act);
        ex.ErrorCode.ShouldBe("FORMAT_TAGNAME");
    }

    [Theory]
    [InlineData("red")]
    [InlineData("#FFFFFZ")]
    [InlineData("FFFFFF")]
    public void Ctor_ShouldThrowPatternException_WhenColorDoesNotMatchRegex(string color)
    {
        // Arrange
        string name = faker.Random.String2(10, "abcdefghijklmnopqrstuvwxyz");

        // Act
        var act = () => new TagValueObject(name, color);

        // Assert
        PatternException ex = Should.Throw<PatternException>(act);
        ex.ErrorCode.ShouldBe("FORMAT_COLOTTAG");
    }

    [Fact]
    public void Equals_ShouldBeTrue_WhenAtomicValuesAreEqual()
    {
        // Arrange
        string name = faker.Random.String2(10, "abcdefghijklmnopqrstuvwxyz");
        string color = "#A1B2C3";
        var a = new TagValueObject(name, color);
        var b = new TagValueObject(name, color);

        // Act
        bool equals = a == b;

        // Assert
        equals.ShouldBeTrue();
        a.GetHashCode().ShouldBe(b.GetHashCode());
    }

    [Fact]
    public void Ctor_ShouldCreate_WhenInputsAreValid()
    {
        // Arrange
        string name = faker.Random.String2(10, "abcdefghijklmnopqrstuvwxyz");
        string color = "#" + faker.Random.String2(6, "0123456789ABCDEF");

        // Act
        var tag = new TagValueObject(name, color);

        // Assert
        tag.Name.ShouldBe(name);
        tag.Color.ShouldBe(color);
    }
}

