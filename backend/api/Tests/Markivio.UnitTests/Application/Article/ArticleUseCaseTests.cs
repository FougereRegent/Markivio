using Bogus;
using FluentResults;
using Markivio.Application.Dto;
using Markivio.Application.Errors;
using Markivio.Application.UseCases;
using Markivio.Domain.Auth;
using Markivio.Domain.Entities;
using Markivio.Domain.Errors;
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
        var existing = new Article(
            new ArticleContent(faker.Internet.Url(), faker.Lorem.Paragraph(), new List<TagValueObject>(), null),
            faker.Lorem.Slug(4))
        { Id = Guid.NewGuid() };

        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(ValueTask.FromResult<Article?>(existing));

        // Act
        Result<ArticleInformation> result = await useCase.CreateArticle(
            new CreateArticle("title", faker.Internet.Url(), "desc", Array.Empty<TagCreateArticle>()));

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<AlreadyExistError>();
    }

    [Fact]
    public async Task CreateArticle_ShouldFail_WhenSomeTagsDoNotExist()
    {
        // Arrange
        Guid[] tagIds = Enumerable.Range(0, 3).Select(_ => Guid.NewGuid()).ToArray();
        CreateArticle input = new(
            Title: faker.Random.Word(),
            Source: faker.Internet.Url(),
            Description: faker.Lorem.Sentence(),
            Tags: tagIds.Select(id => new TagCreateArticle(id)).ToArray());

        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(ValueTask.FromResult<Article?>(null));

        tagUseCaseMock.Setup(pre => pre.TagsExist<Guid>(It.IsAny<IEnumerable<Guid>>(), TagExistConditionEnum.Id))
          .Returns(false);

        // Act
        Result<ArticleInformation> result = await useCase.CreateArticle(input);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NotFoundError>();
    }

    [Fact]
    public async Task CreateArticle_ShouldFail_WhenCurrentUserIsNull()
    {
        // Arrange
        Guid[] tagIds = Enumerable.Range(0, 2).Select(_ => Guid.NewGuid()).ToArray();
        CreateArticle input = new(
            Title: faker.Random.Word(),
            Source: faker.Internet.Url(),
            Description: faker.Lorem.Sentence(),
            Tags: tagIds.Select(id => new TagCreateArticle(id)).ToArray());

        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(ValueTask.FromResult<Article?>(null));

        tagUseCaseMock.Setup(pre => pre.TagsExist<Guid>(It.IsAny<IEnumerable<Guid>>(), TagExistConditionEnum.Id))
          .Returns(true);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
          .Returns(tagIds.Select(id => CreateTag(id, $"Tag{id.ToString()[..6]}")).AsQueryable());

        authUserMock.Setup(pre => pre.CurrentUser).Returns((User)null!);

        // Act
        Result<ArticleInformation> result = await useCase.CreateArticle(input);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NullFieldError>();
    }

    [Fact]
    public async Task CreateArticle_ShouldFail_WhenSourceIsEmpty()
    {
        // Arrange
        Guid[] tagIds = Enumerable.Range(0, 2).Select(_ => Guid.NewGuid()).ToArray();
        CreateArticle input = new(
            Title: faker.Random.Word(),
            Source: "",
            Description: faker.Lorem.Sentence(),
            Tags: tagIds.Select(id => new TagCreateArticle(id)).ToArray());

        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(ValueTask.FromResult<Article?>(null));

        tagUseCaseMock.Setup(pre => pre.TagsExist<Guid>(It.IsAny<IEnumerable<Guid>>(), TagExistConditionEnum.Id))
          .Returns(true);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
          .Returns(tagIds.Select(id => CreateTag(id, $"Tag{id.ToString()[..6]}")).AsQueryable());

        authUserMock.Setup(pre => pre.CurrentUser).Returns(CreateValidUser());

        // Act
        Result<ArticleInformation> result = await useCase.CreateArticle(input);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<ShouldNotBeEmptyError>();
    }

    [Fact]
    public async Task CreateArticle_ShouldCreate_WhenInputsAreValid()
    {
        // Arrange
        Faker localFaker = new Faker("fr");
        string title = localFaker.Random.Word();
        string url = localFaker.Internet.Url();
        Guid[] tagIds = Enumerable.Range(0, 2).Select(_ => Guid.NewGuid()).ToArray();

        CreateArticle input = new(
            Title: title,
            Source: url,
            Description: localFaker.Lorem.Sentence(),
            Tags: tagIds.Select(id => new TagCreateArticle(id)).ToArray());

        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(ValueTask.FromResult<Article?>(null));

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
        Result<ArticleInformation> result = await useCase.CreateArticle(input);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Title.ShouldBe(title);
        result.Value.Source.ShouldBe(url);
        result.Value.Id.ShouldNotBe(Guid.Empty);
        articleRepositoryMock.Verify(pre => pre.Save(It.IsAny<Article>()), Times.Once());
    }
}

