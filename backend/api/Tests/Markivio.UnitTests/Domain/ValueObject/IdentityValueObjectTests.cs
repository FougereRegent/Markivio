using Markivio.Domain.Exceptions;
using Markivio.Domain.ValueObject;
using Shouldly;

namespace Markivio.UnitTests.Domain.ValueObject;

public sealed class IdentityValueObjectTests : BaseTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Ctor_ShouldThrowEmptyException_WhenFirstNameIsNullOrEmpty(string? firstName)
    {
        // Arrange
        string userName = faker.Internet.UserName();
        string lastName = faker.Person.LastName;

        // Act
        var act = () => new IdentityValueObject(userName, firstName!, lastName);

        // Assert
        EmptyException ex = Should.Throw<EmptyException>(act);
        ex.ErrorCode.ShouldBe("EMPTY_FIRSTNAME");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Ctor_ShouldThrowEmptyException_WhenLastNameIsNullOrEmpty(string? lastName)
    {
        // Arrange
        string userName = faker.Internet.UserName();
        string firstName = faker.Person.FirstName;

        // Act
        var act = () => new IdentityValueObject(userName, firstName, lastName!);

        // Assert
        EmptyException ex = Should.Throw<EmptyException>(act);
        ex.ErrorCode.ShouldBe("EMPTY_LASTNAME");
    }

    [Theory]
    [InlineData("&^^")]
    [InlineData("1233654")]
    [InlineData("Jenan&jen")]
    public void Ctor_ShouldThrowPatternException_WhenFirstNameIsInBadFormat(string firstName)
    {
        // Arrange
        string userName = faker.Internet.UserName();
        string lastName = faker.Person.LastName;

        // Act
        var act = () => new IdentityValueObject(userName, firstName, lastName);

        // Assert
        PatternException ex = Should.Throw<PatternException>(act);
        ex.ErrorCode.ShouldBe("FORMAT_FIRSTNAME");
    }

    [Theory]
    [InlineData("&^^")]
    [InlineData("1233654")]
    [InlineData("Jenan&jen")]
    public void Ctor_ShouldThrowPatternException_WhenLastNameIsInBadFormat(string lastName)
    {
        // Arrange
        string userName = faker.Internet.UserName();
        string firstName = faker.Person.FirstName;

        // Act
        var act = () => new IdentityValueObject(userName, firstName, lastName);

        // Assert
        PatternException ex = Should.Throw<PatternException>(act);
        ex.ErrorCode.ShouldBe("FORMAT_LASTNAME");
    }

    [Fact]
    public void Ctor_ShouldCreate_WhenInputsAreValid()
    {
        // Arrange
        string userName = faker.Internet.UserName();
        string firstName = faker.Person.FirstName;
        string lastName = faker.Person.LastName;

        // Act
        var vo = new IdentityValueObject(userName, firstName, lastName);

        // Assert
        vo.Username.ShouldBe(userName);
        vo.FirstName.ShouldBe(firstName);
        vo.LastName.ShouldBe(lastName);
    }
}

