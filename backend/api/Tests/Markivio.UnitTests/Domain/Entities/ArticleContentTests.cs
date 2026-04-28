using Markivio.Domain.Entities;
using Markivio.Domain.Exceptions;
using Markivio.Domain.ValueObject;
using Markivio.UnitTests.Helper.Faker;
using Shouldly;

namespace Markivio.UnitTests.Domain.Entities;

public sealed class ArticleContentTests : BaseTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Ctor_ShouldThrowEmptyException_WhenSourceIsNullOrEmpty(string? source)
    {
        // Arrange
        string content = faker.Lorem.Paragraph();

        // Act
        var act = () => new ArticleContent(source!, content, description: null);

        // Assert
        EmptyException ex = Should.Throw<EmptyException>(act);
        ex.ErrorCode.ShouldBe("EMPTY_ARTICLESOURCE");
    }

    [Theory]
    [InlineData("localhost")]
    [InlineData("http://localhost")]
    [InlineData("not a url")]
    public void Ctor_ShouldThrowPatternException_WhenSourceDoesNotMatchUrlPattern(string source)
    {
        // Arrange
        string content = faker.Lorem.Paragraph();

        // Act
        var act = () => new ArticleContent(source, content, description: null);

        // Assert
        PatternException ex = Should.Throw<PatternException>(act);
        ex.ErrorCode.ShouldBe("FORMAT_ARTICLE_SOURCE");
    }

    [Fact]
    public void UpdateTags_ShouldUpdate()
    {
        //Arrange
        string content = faker.Lorem.Paragraph();
        ArticleContent article = new ArticleContent("https://www.google.com", content, description: null);
        string description = faker.Lorem.Paragraph();

        //Act
        article.Update(description: description);
        //Assert
        article.Description.ShouldBe(description);
    }
}

