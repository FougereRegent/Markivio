using Markivio.Domain.Entities;
using Markivio.Domain.Exceptions;
using Markivio.Domain.ValueObject;
using Shouldly;

namespace Markivio.UnitTests.Domain.Entities;

public sealed class ArticleTests : BaseTests
{
    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenArticleContentIsNull()
    {
        // Arrange
        ArticleContent articleContent = null!;
        string title = faker.Lorem.Slug(wordcount: 4);

        // Act
        var act = () => new Article(articleContent, title, false);

        // Assert
        Should.Throw<ArgumentNullException>(act).ParamName.ShouldBe("articleContent");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Ctor_ShouldThrowEmptyException_WhenTitleIsNullOrEmpty(string? title)
    {
        // Arrange
        var content = new ArticleContent(
            source: faker.Internet.Url(),
            content: faker.Lorem.Paragraph(),
            tags: new List<TagValueObject>(),
            description: null);

        // Act
        var act = () => new Article(content, title!, false);

        // Assert
        EmptyException ex = Should.Throw<EmptyException>(act);
        ex.ErrorCode.ShouldBe("EMPTY_ARTICLETITLE");
    }

    [Fact]
    public void Ctor_ShouldCreate_WhenInputsAreValid()
    {
        // Arrange
        string title = faker.Lorem.Slug(wordcount: 4);
        var content = new ArticleContent(
            source: faker.Internet.Url(),
            content: faker.Lorem.Paragraph(),
            tags: new List<TagValueObject>(),
            description: faker.Lorem.Sentence());

        // Act
        var article = new Article(content, title, false);

        // Assert
        article.Title.ShouldBe(title);
        article.ArticleContent.ShouldBeSameAs(content);
    }

    [Fact]
    public void ArticleUpdate_ShouldNotThrow_WhenTitleIsEmpty()
    {
        //Arrange
        var title = faker.Lorem.Slug(wordcount: 4);
        var content = new ArticleContent(
            source: faker.Internet.Url(),
            content: faker.Lorem.Paragraph(),
            tags: new List<TagValueObject>(),
            description: faker.Lorem.Sentence());
        var article = new Article(content, title, true);

        //Act
        var act = () => article.Update(title: string.Empty, description: faker.Lorem.Sentence(), source: faker.Internet.Url(), isFramable: true, tags: new List<TagValueObject>());

        //Assert
        var ex = Should.Throw<EmptyException>(act);
        ex.ErrorCode.ShouldBe("EMPTY_ARTICLETITLE");
    }

    [Fact]
    public void ArticleUpdate_ShouldUpdate()
    {
        //Arrange
        var title = faker.Lorem.Slug(wordcount: 4);
        var content = new ArticleContent(
            source: faker.Internet.Url(),
            content: faker.Lorem.Paragraph(),
            tags: new List<TagValueObject>(),
            description: faker.Lorem.Sentence());
        var article = new Article(content, title, true);
        var updatedTitle = faker.Lorem.Slug(wordcount: 4);
        //Act
        article.Update(title: updatedTitle, description: faker.Lorem.Sentence(), source: faker.Internet.Url(), isFramable: true, tags: new List<TagValueObject>());

        //Assert
        article.Title.ShouldBe(updatedTitle);
    }
}

