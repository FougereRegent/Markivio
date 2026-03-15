using Markivio.Domain.Exceptions;
using Markivio.Domain.ValueObject;
using Shouldly;

namespace Markivio.UnitTests.Domain.ValueObject;

public sealed class EmailValueObjectTests : BaseTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Ctor_ShouldThrowEmptyException_WhenEmailIsNullOrEmpty(string? email)
    {
        // Arrange
        // Act
        var act = () => new EmailValueObject(email!);

        // Assert
        EmptyException ex = Should.Throw<EmptyException>(act);
        ex.ErrorCode.ShouldBe("EMPTY_EMAIL");
    }

    [Theory]
    [InlineData("test")]
    [InlineData("test2@")]
    [InlineData("&&&&@.fr")]
    [InlineData("damien.Venant&com")]
    public void Ctor_ShouldThrowPatternException_WhenEmailIsInBadFormat(string email)
    {
        // Arrange
        // Act
        var act = () => new EmailValueObject(email);

        // Assert
        PatternException ex = Should.Throw<PatternException>(act);
        ex.ErrorCode.ShouldBe("FORMAT_EMAIL");
    }

    [Fact]
    public void Ctor_ShouldCreate_WhenEmailIsValid()
    {
        // Arrange
        string email = faker.Internet.Email();

        // Act
        var vo = new EmailValueObject(email);

        // Assert
        vo.Email.ShouldBe(email);
    }
}

