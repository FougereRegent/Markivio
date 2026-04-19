using Bogus;
using FluentResults;
using Markivio.Application.Dto;
using Markivio.Application.Errors;
using Markivio.Application.UseCases;
using Markivio.Domain.Auth;
using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Domain.ValueObject;
using Moq;
using Shouldly;

namespace Markivio.UnitTests.Application;

public sealed class ArticleUseCaseTests : BaseTests
{
    private readonly Mock<IAuthUser> authUserMock = new();
    private readonly Mock<ITagUseCase> tagUseCaseMock = new();
    private readonly Mock<IArticleRepository> articleRepositoryMock = new();
    private readonly Mock<ITagRepository> tagRepositoryMock = new();

    private readonly ArticleUseCase useCase;

    public ArticleUseCaseTests()
    {
        useCase = new ArticleUseCase(
            tagUseCaseMock.Object,
            articleRepositoryMock.Object,
            tagRepositoryMock.Object,
            authUserMock.Object);
    }

    private User CreateValidUser()
    {
        var identity = new IdentityValueObject(
            userName: faker.Internet.UserName(),
            firstName: faker.Person.FirstName,
            lastName: faker.Person.LastName);
        var email = new EmailValueObject(faker.Internet.Email());
        return new User(identity, email) { Id = Guid.NewGuid(), AuthId = faker.Random.Guid().ToString() };
    }

    private Tag CreateTag(Guid id, string name)
        => new Tag(new TagValueObject(name, "#FFFFFF")) { Id = id };

    [Fact]
    public async Task CreateArticle_ShouldFail_WhenArticleAlreadyExists()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        var existing = new Article(
            new ArticleContent(faker.Internet.Url(), faker.Lorem.Paragraph(), new List<TagValueObject>(), null),
            faker.Lorem.Slug(4), false)
        { Id = Guid.NewGuid() };

        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(Task.FromResult<Article?>(existing));

        // Act
        Result<ArticleInformation> result = await useCase.CreateArticle(
            new CreateArticle("title", faker.Internet.Url(), "desc", Array.Empty<TagArticle>()), token);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<AlreadyExistError>();
    }

    [Fact]
    public async Task CreateArticle_ShouldFail_WhenSomeTagsDoNotExist()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        Guid[] tagIds = Enumerable.Range(0, 3).Select(_ => Guid.NewGuid()).ToArray();
        CreateArticle input = new(
            Title: faker.Random.Word(),
            Source: faker.Internet.Url(),
            Description: faker.Lorem.Sentence(),
            Tags: tagIds.Select(id => new TagArticle(id)).ToArray());

        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(Task.FromResult<Article?>(null));

        tagUseCaseMock.Setup(pre => pre.TagsExist<Guid>(It.IsAny<IEnumerable<Guid>>(), TagExistConditionEnum.Id))
          .Returns(false);

        // Act
        Result<ArticleInformation> result = await useCase.CreateArticle(input, token);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NotFoundError>();
    }

    [Fact]
    public async Task CreateArticle_ShouldFail_WhenSourceIsEmpty()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        Guid[] tagIds = Enumerable.Range(0, 2).Select(_ => Guid.NewGuid()).ToArray();
        CreateArticle input = new(
            Title: faker.Random.Word(),
            Source: "",
            Description: faker.Lorem.Sentence(),
            Tags: tagIds.Select(id => new TagArticle(id)).ToArray());

        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(Task.FromResult<Article?>(null));

        tagUseCaseMock.Setup(pre => pre.TagsExist<Guid>(It.IsAny<IEnumerable<Guid>>(), TagExistConditionEnum.Id))
          .Returns(true);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
          .Returns(tagIds.Select(id => CreateTag(id, $"Tag{id.ToString()[..6]}")).AsQueryable());

        authUserMock.Setup(pre => pre.CurrentUser).Returns(CreateValidUser());

        // Act
        Result<ArticleInformation> result = await useCase.CreateArticle(input, token);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<DomainError>();
        result.Errors[0].Metadata[ErrorCode.ERROR_CODE_PROPERTY_NAME].ShouldBe("EMPTY_ARTICLESOURCE");
    }

    [Fact]
    public async Task CreateArticle_ShouldCreate_WhenInputsAreValid()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        Faker localFaker = new Faker("fr");
        string title = localFaker.Random.Word();
        string url = localFaker.Internet.Url();
        Guid[] tagIds = Enumerable.Range(0, 2).Select(_ => Guid.NewGuid()).ToArray();

        CreateArticle input = new(
            Title: title,
            Source: url,
            Description: localFaker.Lorem.Sentence(),
            Tags: tagIds.Select(id => new TagArticle(id)).ToArray());

        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(Task.FromResult<Article?>(null));

        tagUseCaseMock.Setup(pre => pre.TagsExist<Guid>(It.IsAny<IEnumerable<Guid>>(), TagExistConditionEnum.Id))
          .Returns(true);

        Tag[] dbTags = tagIds.Select(id => CreateTag(id, $"Tag{id.ToString()[..6]}")).ToArray();
        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
          .Returns(dbTags.AsQueryable());

        User currentUser = CreateValidUser();
        authUserMock.Setup(pre => pre.CurrentUser).Returns(currentUser);

        articleRepositoryMock.Setup(pre => pre.Save(It.IsAny<Article>()))
          .Returns((Article a) =>
          {
              a.Id = Guid.NewGuid();
              return a;
          });

        // Act
        Result<ArticleInformation> result = await useCase.CreateArticle(input, token);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Title.ShouldBe(title);
        result.Value.Source.ShouldBe(url);
        result.Value.Id.ShouldNotBe(Guid.Empty);
        articleRepositoryMock.Verify(pre => pre.Save(It.IsAny<Article>()), Times.Once());
    }
    [Fact]
    public async Task UpdateArticle_ShouldFail_WhenArticleDoesNotExist()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        UpdateArticle input = new(
            Id: Guid.NewGuid(),
            Title: faker.Random.Word(),
            Source: faker.Internet.Url(),
            Description: faker.Lorem.Sentence(),
            Tags: Array.Empty<TagArticle>());

        articleRepositoryMock.Setup(pre => pre.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article?)null);

        // Act
        Result<ArticleInformation> result = await useCase.UpdateArticle(input, token);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NotFoundError>();
    }

    [Fact]
    public async Task UpdateArticle_ShouldUseExistingFramable_WhenSourceIsUnchanged()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        string source = faker.Internet.Url();

        Article article = new(
            new ArticleContent(source, faker.Lorem.Paragraph(), new List<TagValueObject>(), null),
            faker.Lorem.Slug(),
            true)
        { Id = Guid.NewGuid(), User = CreateValidUser() };

        UpdateArticle input = new(
            Id: article.Id,
            Title: faker.Random.Word(),
            Source: source, // même source
            Description: faker.Lorem.Sentence(),
            Tags: Array.Empty<TagArticle>());

        articleRepositoryMock.Setup(pre => pre.GetById(article.Id, token))
            .ReturnsAsync(article);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(Enumerable.Empty<Tag>().AsQueryable());

        // Act
        Result<ArticleInformation> result = await useCase.UpdateArticle(input, token);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        articleRepositoryMock.Verify(pre => pre.IsFramable(It.IsAny<string>()), Times.Never());
    }
    [Fact]
    public async Task UpdateArticle_ShouldCallIsFramable_WhenSourceChanged()
    {
        // Arrange
        CancellationToken token = new CancellationToken();

        Article article = new(
            new ArticleContent(faker.Internet.Url(), faker.Lorem.Paragraph(), new List<TagValueObject>(), null),
            faker.Lorem.Slug(),
            false)
        { Id = Guid.NewGuid(), User = CreateValidUser() };

        string newSource = faker.Internet.Url();

        UpdateArticle input = new(
            Id: article.Id,
            Title: faker.Random.Word(),
            Source: newSource,
            Description: faker.Lorem.Sentence(),
            Tags: Array.Empty<TagArticle>());

        articleRepositoryMock.Setup(pre => pre.GetById(article.Id, token))
            .ReturnsAsync(article);

        articleRepositoryMock.Setup(pre => pre.IsFramable(newSource))
            .ReturnsAsync(true);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(Enumerable.Empty<Tag>().AsQueryable());

        // Act
        Result<ArticleInformation> result = await useCase.UpdateArticle(input, token);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        articleRepositoryMock.Verify(pre => pre.IsFramable(newSource), Times.Once());
    }

    [Fact]
    public async Task UpdateArticle_ShouldFail_WhenDomainExceptionThrown()
    {
        // Arrange
        CancellationToken token = new CancellationToken();

        Article article = new(
            new ArticleContent(faker.Internet.Url(), faker.Lorem.Paragraph(), new List<TagValueObject>(), null),
            faker.Lorem.Slug(),
            false)
        { Id = Guid.NewGuid(), User = CreateValidUser() };

        UpdateArticle input = new(
            Id: article.Id,
            Title: "", // provoque une erreur métier (ex: titre vide)
            Source: faker.Internet.Url(),
            Description: faker.Lorem.Sentence(),
            Tags: Array.Empty<TagArticle>());

        articleRepositoryMock.Setup(pre => pre.GetById(article.Id, token))
            .ReturnsAsync(article);

        articleRepositoryMock.Setup(pre => pre.IsFramable(It.IsAny<string>()))
            .ReturnsAsync(true);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(Enumerable.Empty<Tag>().AsQueryable());

        // Act
        Result<ArticleInformation> result = await useCase.UpdateArticle(input, token);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<DomainError>();
    }
    [Fact]
    public async Task UpdateArticle_ShouldUpdateArticle_WhenInputsAreValid()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        Faker localFaker = new Faker("fr");

        Article article = new(
            new ArticleContent(localFaker.Internet.Url(), localFaker.Lorem.Paragraph(), new List<TagValueObject>(), null),
            localFaker.Lorem.Slug(),
            false)
        { Id = Guid.NewGuid(), User = CreateValidUser() };

        Guid[] tagIds = Enumerable.Range(0, 2).Select(_ => Guid.NewGuid()).ToArray();

        UpdateArticle input = new(
            Id: article.Id,
            Title: localFaker.Random.Word(),
            Source: localFaker.Internet.Url(),
            Description: localFaker.Lorem.Sentence(),
            Tags: tagIds.Select(id => new TagArticle(id)).ToArray());

        Tag[] dbTags = tagIds.Select(id => CreateTag(id, $"Tag{id.ToString()[..6]}")).ToArray();

        articleRepositoryMock.Setup(pre => pre.GetById(article.Id, token))
            .ReturnsAsync(article);

        articleRepositoryMock.Setup(pre => pre.IsFramable(It.IsAny<string>()))
            .ReturnsAsync(true);

        articleRepositoryMock.Setup(pre => pre.Update(It.IsAny<Article>()))
            .Returns(article);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(dbTags.AsQueryable());


        // Act
        Result<ArticleInformation> result = await useCase.UpdateArticle(input, token);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldBe(article.Id);
        result.Value.Title.ShouldBe(input.Title);
        result.Value.Source.ShouldBe(input.Source);
    }
}
