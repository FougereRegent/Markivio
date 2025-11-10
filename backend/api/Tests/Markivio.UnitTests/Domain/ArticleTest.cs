using Bogus;
using FluentResults;
using Markivio.Domain.Entities;
using Markivio.Domain.Errors;
using Shouldly;

namespace Markivio.UnitTests.Domain;

public class ArticleTests : BaseTests
{

    [Fact]
    public void ArticleValidation_ShouldNotValidate_WhenUserIsNull()
    {
        //Arrange
        string url = faker.Internet.Url();
        Article article = new Article
        {
            Source = url,
            Title = faker.Lorem.Slug(wordcount: 4),
            Content = faker.Lorem.Paragraph(),
            User = null!,
        };
        //Act
        Result result = article.Validate();

        //Assert
        Assert.Multiple(() =>
        {
            result.IsFailed.ShouldBeTrue();
            if (result.Errors.Count > 1)
                url.ShouldBe("");
            result.Errors[0].ShouldBeOfType<NullFieldError>();
            result.Errors[0].Message.ShouldBe("User cannot be null");
        });
    }

    [Theory]
    [InlineData(""), InlineData(null)]
    public void ArticleValidation_ShouldNotValidate_WhenTitleIsNullOrEmpty(string? title)
    {
        //Arrange
        Article article = new Article
        {
            Source = faker.Internet.Url(),
            Title = title!,
            Content = faker.Lorem.Paragraph(),
            User = new User { Id = Guid.NewGuid() }
        };
        //Act
        Result result = article.Validate();

        //Assert
        Assert.Multiple(() =>
        {
            result.IsFailed.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].Message.ShouldBe("Title should not be empty");
        });
    }

    [Theory]
    [InlineData(""), InlineData("hhttp://test.com"), InlineData("ttps://test.fr"), InlineData(null)]
    public void ArticleValidation_SholdNotValidation_WhenUrlIsNotUrl(string? url)
    {
        //Arrange
        Article article = new Article
        {
            Source = url!,
            Title = faker.Lorem.Slug(wordcount: 4),
            Content = faker.Lorem.Paragraph(),
            User = new User { Id = Guid.NewGuid() }
        };
        //Act
        Result result = article.Validate();

        //Assert
        result.IsFailed.ShouldBeTrue();
    }

    public static IEnumerable<object[]> GetArticles()
    {
        for (int i = 0; i < 10; ++i)
        {
            Faker faker = new Faker();
            yield return new object[] { faker.Internet.Url(), faker.Lorem.Slug(wordcount: 4), faker.Lorem.Paragraph() };
        }
    }
    [Theory]
    [MemberData(nameof(GetArticles))]
    public void ArticleValidation_ShouldValidate(string url, string title, string content)
    {
        //Arrange
        Article article = new Article
        {
            Source = url,
            Title = title,
            Content = content,
            User = new User { Id = Guid.NewGuid() }
        };

        //Act
        Result result = article.Validate();

        //Assert
        result.IsSuccess.ShouldBeTrue();
    }
}
