using Markivio.Domain.Entities;
using Markivio.Domain.Exceptions;
using Markivio.UnitTests.Helper.Faker;
using Shouldly;

namespace Markivio.UnitTests.Domain.Entities;

public sealed class ArticleTests : BaseTests
{
    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenArticleContentIsNull()
    {
        // Arrange
        ArticleContent articleContent = null!;
        List<Tag> tags = new List<Tag>();
        string title = faker.Lorem.Slug(wordcount: 4);

        // Act
        var act = () => new Article(articleContent, title, false, tags);

        // Assert
        Should.Throw<ArgumentNullException>(act).ParamName.ShouldBe("articleContent");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Ctor_ShouldThrowEmptyException_WhenTitleIsNullOrEmpty(string? title)
    {
        // Arrange
        List<Tag> tags = new List<Tag>();
        ArticleContent content = new ArticleContent(
            source: faker.Internet.Url(),
            content: faker.Lorem.Paragraph(),
            description: null);

        // Act
        var act = () => new Article(content, title!, false, tags);

        // Assert
        EmptyException ex = Should.Throw<EmptyException>(act);
        ex.ErrorCode.ShouldBe("EMPTY_ARTICLETITLE");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Ctor_ShouldSetIsFramable(bool isFramable)
    {
        // Arrange
        ArticleContent articleContent = ArticleContentGenerator.CreateValid();
        string title = faker.Lorem.Slug();
        // act
        Article article = new Article(articleContent: articleContent, title: title, isFramable: isFramable, tags: new List<Tag>());
        // Assert
        article.IsFramable.ShouldBe(isFramable);
    }

    public void Ctor_CreatingSuccess()
    {
        // Arrange
        ArticleContent articleContent = ArticleContentGenerator.CreateValid();
        string title = faker.Lorem.Slug();
        bool isFramable = faker.Random.Bool();
        List<Tag> tags = new List<Tag>();
        // Act
        Article article = new Article(articleContent: articleContent, title: title, isFramable: isFramable, tags: tags);
        // Assert
        article.ArticleContent.ShouldBe(articleContent);
        article.Title.ShouldBe(title);
        article.IsFramable.ShouldBe(isFramable);
        article.Tags.ShouldBe(tags);
    }

    [Fact]
    public void Ctor_ShouldCreate_WhenInputsAreValid()
    {
        // Arrange
        string title = faker.Lorem.Slug(wordcount: 4);
        List<Tag> tags = new List<Tag>();
        var content = new ArticleContent(
            source: faker.Internet.Url(),
            content: faker.Lorem.Paragraph(),
            description: faker.Lorem.Sentence());

        // Act
        var article = new Article(content, title, false, tags);

        // Assert
        article.Title.ShouldBe(title);
        article.ArticleContent.ShouldBeSameAs(content);
    }

    [Fact]
    public void Ctor_ShouldThrowTagLimitExceededException_WhenTagsExceedLimit()
    {
        // Arrange
        string source = faker.Internet.Url();
        string content = faker.Lorem.Paragraph();
        List<Tag> tags = Enumerable.Range(0, 21).Select(_ => TagValueGenerator.CreateValidTag()).ToList();

        ArticleContent articleContent = new ArticleContent(
                source: source,
                description: null,
                content: content
                );

        // Act
        var act = () => new Article(articleContent, "title", false, tags);

        // Assert
        Should.Throw<TagLimitExceededException>(act).ErrorCode.ShouldBe("TAG_LIMIT_EXCEEDED");
    }

    [Fact]
    public void ArticleUpdate_ShouldNotThrow_WhenTitleIsEmpty()
    {
        //Arrange
        List<Tag> tags = new List<Tag>();
        ArticleContent content = new ArticleContent(
            source: faker.Internet.Url(),
            content: faker.Lorem.Paragraph(),
            description: faker.Lorem.Sentence());
        string title = faker.Lorem.Slug(wordcount: 4);
        Article article = new Article(content, title, true, tags);

        //Act
        var act = () => article.Update(title: string.Empty, description: faker.Lorem.Sentence(), tags: new List<Tag>());

        //Assert
        var ex = Should.Throw<EmptyException>(act);
        ex.ErrorCode.ShouldBe("EMPTY_ARTICLETITLE");
    }

    [Fact]
    public void ArticleUpdate_ShouldUpdate()
    {
        //Arrange
        List<Tag> tags = new List<Tag>();
        string title = faker.Lorem.Slug(wordcount: 4);
        ArticleContent content = new ArticleContent(
            source: faker.Internet.Url(),
            content: faker.Lorem.Paragraph(),
            description: faker.Lorem.Sentence());
        Article article = new Article(content, title, true, tags);
        string updatedTitle = faker.Lorem.Slug(wordcount: 4);
        //Act
        article.Update(title: updatedTitle, description: faker.Lorem.Sentence(), tags: new List<Tag>());

        //Assert
        article.Title.ShouldBe(updatedTitle);
    }

}

