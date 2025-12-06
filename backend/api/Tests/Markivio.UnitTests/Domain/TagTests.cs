
using Bogus;
using FluentResults;
using Markivio.Domain.Entities;
using Shouldly;

namespace Markivio.UnitTests.Domain;

public class TagTests : BaseTests
{
    [Theory]
    [InlineData("!!::"), InlineData("test!"), InlineData("???")]
    public void TagValidation_ShouldNotValidate_WhenNameDoesntMatchRegex(string tagName)
    {
        //Arrange
        Tag tag = new Tag
        {
            Name = tagName,
            Color = "#FFFFFF" //White color in hex
        };

        //Act
        Result result = tag.Validate();

        //Assert
        result.IsFailed.ShouldBe(true);
    }

    [Theory]
    [InlineData("red"), InlineData("#FFFFFZ"), InlineData("FFFFFF")]
    public void TagValidation_ShouldNotValidate_WhenColorDoesntMatchRegex(string color)
    {
        //Arrange
        Tag tag = new Tag
        {
            Name = "test",
            Color = color,
        };
        //Act
        Result result = tag.Validate();

        //Assert
        result.IsFailed.ShouldBe(true);
    }

    public static IEnumerable<object[]> GetTags()
    {
        for (int i = 0; i < 10; ++i)
        {
            Faker faker = new Faker("fr");
            yield return new object[] {
              faker.Lorem.Word(),
              "#" + faker.Random.String2(6, "123456789ABCDEFabcdef")
            };
        }
    }

    [Theory]
    [MemberData(nameof(GetTags))]
    public void TagValidation_ShouldValidate(string tagName, string color)
    {
        //Arrange
        Tag tag = new Tag
        {
            Name = tagName,
            Color = color
        };
        //Assert
        Result result = tag.Validate();

        //Act
        result.IsSuccess.ShouldBe(true);
    }
}
