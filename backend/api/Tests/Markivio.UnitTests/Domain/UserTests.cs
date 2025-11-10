using Bogus;
using FluentResults;
using Markivio.Domain.Entities;
using Markivio.Domain.Errors;
using Shouldly;

namespace Markivio.UnitTests.Domain;

public class UserTests : BaseTests
{
    [Theory]
    [InlineData("test"), InlineData("test2@"), InlineData("&&&&@.fr"), InlineData("damien.Venant&com")]
    public void UserValidation_ShouldNotValidate_WhenEmailIsInBadFormat(string email)
    {
        //Arrange
        User user = new User
        {
            Email = email,
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName,
        };
        //Act
        Result result = user.Validate();

        //Assert
        Assert.Multiple(() =>
        {
            result.IsFailed.ShouldBeTrue();
            result.Errors[0].ShouldBeOfType<FormatUnexpectedError>();
            result.Errors[0].Message.ShouldBe("Email is in bad format");
        });
    }

    [Theory]
    [InlineData("&^^"), InlineData("1233654"), InlineData("Jenan&jen"), InlineData("")]
    public void UserValidation_ShouldNotValidate_WhenFirstNameIsInBadFormat(string firstName)
    {
        //Arrange
        User user = new User
        {
            Email = faker.Person.Email,
            FirstName = firstName,
            LastName = faker.Person.LastName,
        };
        //Act
        Result result = user.Validate();

        //Assert
        Assert.Multiple(() =>
        {
            result.IsFailed.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ShouldBeOfType<FormatUnexpectedError>();
            result.Errors[0].Message.ShouldBe("FirstName is in bad format");
        });

    }

    [Theory]
    [InlineData("&^^"), InlineData("1233654"), InlineData("Jenan&jen"), InlineData("")]
    public void UserValidation_ShouldNotValidate_WhenLastNameIsInBadFormat(string lastName)
    {
        //Arrange
        User user = new User
        {
            Email = faker.Person.Email,
            FirstName = faker.Person.FirstName,
            LastName = lastName
        };

        //Act
        Result result = user.Validate();

        //Assert
        Assert.Multiple(() =>
        {
            result.IsFailed.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ShouldBeOfType<FormatUnexpectedError>();
            result.Errors[0].Message.ShouldBe("LastName is in bad format");
        });
    }

    public static IEnumerable<object[]> GetUser()
    {
        for (int i = 0; i < 10; ++i)
        {
            Faker faker = new Faker("fr");
            yield return new object[]
            {
                faker.Person.Email,
                faker.Person.FirstName,
                faker.Person.LastName,
            };
        }
    }

    [Theory]
    [MemberData(nameof(GetUser))]
    public void UserValidation_ShouldValidate(string email, string firstName, string lastName)
    {
        //Arrange 
        User user = new User
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
        };
        //Act
        Result result = user.Validate();

        //Assert
        result.IsSuccess.ShouldBeTrue();
    }
}

