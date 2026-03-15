using Markivio.Domain.Entities;
using Markivio.Domain.Exceptions;
using Markivio.Domain.ValueObject;
using Shouldly;

namespace Markivio.UnitTests.Domain.Entities;

public sealed class ArticleContentTests : BaseTests
{
    private TagValueObject CreateValidTag(string? name = null, string? color = null)
    {
        string safeName = name ?? faker.Random.String2(10, "abcdefghijklmnopqrstuvwxyz");
        string safeColor = color ?? ("#" + faker.Random.String2(6, "0123456789ABCDEF"));
        return new TagValueObject(safeName, safeColor);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Ctor_ShouldThrowEmptyException_WhenSourceIsNullOrEmpty(string? source)
    {
        // Arrange
        string content = faker.Lorem.Paragraph();
        List<TagValueObject> tags = new();

        // Act
        var act = () => new ArticleContent(source!, content, tags, description: null);

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
        List<TagValueObject> tags = new();

        // Act
        var act = () => new ArticleContent(source, content, tags, description: null);

        // Assert
        PatternException ex = Should.Throw<PatternException>(act);
        ex.ErrorCode.ShouldBe("FORMAT_ARTICLE_SOURCE");
    }

    [Fact]
    public void Ctor_ShouldThrowTagLimitExceededException_WhenTagsExceedLimit()
    {
        // Arrange
        string source = faker.Internet.Url();
        string content = faker.Lorem.Paragraph();
        List<TagValueObject> tags = Enumerable.Range(0, 21).Select(_ => CreateValidTag()).ToList();

        // Act
        var act = () => new ArticleContent(source, content, tags, description: null);

        // Assert
        Should.Throw<TagLimitExceededException>(act).ErrorCode.ShouldBe("TAG_LIMIT_EXCEEDED");
    }

    [Fact]
    public void AddTags_ShouldThrowTagLimitExceededException_WhenItWouldExceedLimit()
    {
        // Arrange
        var articleContent = new ArticleContent(
            source: faker.Internet.Url(),
            content: faker.Lorem.Paragraph(),
            tags: Enumerable.Range(0, 19).Select(_ => CreateValidTag()).ToList(),
            description: null);

        IReadOnlyList<TagValueObject> toAdd = new[] { CreateValidTag(), CreateValidTag() };

        // Act
        var act = () => articleContent.AddTags(toAdd);

        // Assert
        Should.Throw<TagLimitExceededException>(act).ErrorCode.ShouldBe("TAG_LIMIT_EXCEEDED");
    }

    [Fact]
    public void RemoveTags_ShouldRemove_ByName()
    {
        // Arrange
        TagValueObject keep = CreateValidTag(name: "keep", color: "#111111");
        TagValueObject remove = CreateValidTag(name: "remove", color: "#AAAAAA");

        var articleContent = new ArticleContent(
            source: faker.Internet.Url(),
            content: faker.Lorem.Paragraph(),
            tags: new List<TagValueObject> { keep, remove },
            description: null);

        // Act
        articleContent.RemoveTags(new[] { new TagValueObject(name: "remove", color: "#BBBBBB") });

        // Assert
        articleContent.Tags.Count.ShouldBe(1);
        articleContent.Tags[0].ShouldBe(keep);
    }
}

