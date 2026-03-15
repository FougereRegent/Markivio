using Markivio.Domain.Entities;
using Markivio.Domain.ValueObject;
using Shouldly;

namespace Markivio.UnitTests.Domain.Entities;

public sealed class UserEntityTests : BaseTests
{
    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenIdentityIsNull()
    {
        // Arrange
        IdentityValueObject identity = null!;
        var email = new EmailValueObject(faker.Internet.Email());

        // Act
        var act = () => new User(identity, email);

        // Assert
        Should.Throw<ArgumentNullException>(act).ParamName.ShouldBe("identityValue");
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenEmailIsNull()
    {
        // Arrange
        var identity = new IdentityValueObject(
            userName: faker.Internet.UserName(),
            firstName: faker.Person.FirstName,
            lastName: faker.Person.LastName);
        EmailValueObject email = null!;

        // Act
        var act = () => new User(identity, email);

        // Assert
        Should.Throw<ArgumentNullException>(act).ParamName.ShouldBe("emailValue");
    }

    [Fact]
    public void Ctor_ShouldCreate_WhenInputsAreValid()
    {
        // Arrange
        var identity = new IdentityValueObject(
            userName: faker.Internet.UserName(),
            firstName: faker.Person.FirstName,
            lastName: faker.Person.LastName);
        var email = new EmailValueObject(faker.Internet.Email());

        // Act
        var user = new User(identity, email);

        // Assert
        user.Identity.ShouldBe(identity);
        user.Email.ShouldBe(email);
    }
}

