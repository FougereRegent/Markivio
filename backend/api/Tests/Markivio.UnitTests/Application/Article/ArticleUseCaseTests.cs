using Bogus;
using FluentResults;
using Markivio.Application.Dto;
using Markivio.Application.Errors;
using Markivio.Application.Interfaces;
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
    private readonly Mock<IWorkerPublisher<ReadableArticleMessage>> workerMock = new();

    private readonly ArticleUseCase useCase;

    public ArticleUseCaseTests()
    {
        useCase = new ArticleUseCase(
            tagUseCaseMock.Object,
            articleRepositoryMock.Object,
            tagRepositoryMock.Object,
            authUserMock.Object,
            workerMock.Object);
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

    private Article CreateArticle(string? title = null, List<Tag>? tags = null)
    {
        tags ??= new List<Tag>();
        return new Article(
            new ArticleContent(faker.Internet.Url(), faker.Lorem.Paragraph(), null),
            title ?? faker.Lorem.Slug(4), false, tags)
        { Id = Guid.NewGuid(), User = CreateValidUser() };
    }

    // ─── CreateArticle ────────────────────────────────────────────

    [Fact]
    public async Task CreateArticle_ShouldFail_WhenArticleAlreadyExists()
    {
        // Arrange
        List<Tag> tags = new List<Tag>();
        CancellationToken token = new CancellationToken();
        Article existing = CreateArticle();

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

    // ─── UpdateArticle ────────────────────────────────────────────

    [Fact]
    public async Task UpdateArticle_ShouldFail_WhenArticleNotFound()
    {
        // Arrange
        var token = new CancellationToken();
        var input = new UpdateArticle(
            Id: Guid.NewGuid(),
            Title: faker.Random.Word(),
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
    public async Task UpdateArticle_ShouldFail_WhenTitleAlreadyExists()
    {
        // Arrange
        var token = new CancellationToken();
        Guid articleId = Guid.NewGuid();
        Article existing = CreateArticle(title: "Original Title");
        existing.Id = articleId;

        string newTitle = "Different Title";

        var input = new UpdateArticle(
            Id: articleId,
            Title: newTitle,
            Description: faker.Lorem.Sentence(),
            Tags: Array.Empty<TagArticle>());

        articleRepositoryMock.Setup(pre => pre.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(Enumerable.Empty<Tag>().AsQueryable());

        articleRepositoryMock.Setup(pre => pre.GetByTitle(newTitle, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateArticle(title: newTitle));

        // Act
        Result<ArticleInformation> result = await useCase.UpdateArticle(input, token);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<AlreadyExistError>();
    }

    [Fact]
    public async Task UpdateArticle_ShouldFail_WhenTitleIsEmpty()
    {
        // Arrange
        var token = new CancellationToken();
        Guid articleId = Guid.NewGuid();
        Article existing = CreateArticle(title: "Original Title");
        existing.Id = articleId;

        var input = new UpdateArticle(
            Id: articleId,
            Title: "",
            Description: faker.Lorem.Sentence(),
            Tags: Array.Empty<TagArticle>());

        articleRepositoryMock.Setup(pre => pre.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

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
    public async Task UpdateArticle_ShouldUpdate_WhenInputsAreValid()
    {
        // Arrange
        Guid articleId = Guid.NewGuid();
        var token = new CancellationToken();
        Article existing = CreateArticle(title: "Original Title");
        existing.Id = articleId;

        string newTitle = faker.Random.Word();
        string newDescription = faker.Lorem.Sentence();

        var input = new UpdateArticle(
            Id: articleId,
            Title: newTitle,
            Description: newDescription,
            Tags: Array.Empty<TagArticle>());

        articleRepositoryMock.Setup(pre => pre.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(Enumerable.Empty<Tag>().AsQueryable());

        articleRepositoryMock.Setup(pre => pre.Update(It.IsAny<Article>()))
            .Returns((Article a) => a);

        // Act
        Result<ArticleInformation> result = await useCase.UpdateArticle(input, token);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Title.ShouldBe(newTitle);
        articleRepositoryMock.Verify(pre => pre.Update(It.IsAny<Article>()), Times.Once());
        articleRepositoryMock.Verify(pre => pre.SaveAndCommit(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task UpdateArticle_ShouldSendMessage_WhenUpdateSucceeds()
    {
        // Arrange
        Guid articleId = Guid.NewGuid();
        var token = new CancellationToken();
        Article existing = CreateArticle(title: "Original Title");
        existing.Id = articleId;

        var input = new UpdateArticle(
            Id: articleId,
            Title: faker.Random.Word(),
            Description: faker.Lorem.Sentence(),
            Tags: Array.Empty<TagArticle>());

        articleRepositoryMock.Setup(pre => pre.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(Enumerable.Empty<Tag>().AsQueryable());

        articleRepositoryMock.Setup(pre => pre.Update(It.IsAny<Article>()))
            .Returns((Article a) => a);

        // Act
        await useCase.UpdateArticle(input, token);

        // Assert
        workerMock.Verify(pre => pre.SendMessageAsync(It.IsAny<ReadableArticleMessage>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task UpdateArticle_ShouldNotChangeTitle_WhenSameTitleWithDifferentCase()
    {
        // Arrange
        var token = new CancellationToken();
        Guid articleId = Guid.NewGuid();
        string originalTitle = "My Article";
        Article existing = CreateArticle(title: originalTitle);
        existing.Id = articleId;

        var input = new UpdateArticle(
            Id: articleId,
            Title: "my article",
            Description: faker.Lorem.Sentence(),
            Tags: Array.Empty<TagArticle>());

        articleRepositoryMock.Setup(pre => pre.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(Enumerable.Empty<Tag>().AsQueryable());

        articleRepositoryMock.Setup(pre => pre.Update(It.IsAny<Article>()))
            .Returns((Article a) => a);

        // Act
        Result<ArticleInformation> result = await useCase.UpdateArticle(input, token);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        articleRepositoryMock.Verify(pre => pre.GetByTitle(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    // ─── AddTags ──────────────────────────────────────────────────

    [Fact]
    public async Task AddTags_ShouldFail_WhenArticleNotFound()
    {
        // Arrange
        var input = new AddTagsToArticle(
            articleId: Guid.NewGuid(),
            tagIds: new[] { Guid.NewGuid() });

        articleRepositoryMock.Setup(pre => pre.GetById(It.IsAny<Guid>()))
            .ReturnsAsync((Article?)null);

        // Act
        Result<ArticleInformation> result = await useCase.AddTags(input);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NotFoundError>();
    }

    [Fact]
    public async Task AddTags_ShouldFail_WhenTagsNotFound()
    {
        // Arrange
        Article article = CreateArticle();
        Guid[] tagIds = { Guid.NewGuid(), Guid.NewGuid() };

        var input = new AddTagsToArticle(
            articleId: article.Id,
            tagIds: tagIds);

        articleRepositoryMock.Setup(pre => pre.GetById(article.Id))
            .ReturnsAsync(article);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(new List<Tag> { CreateTag(Guid.NewGuid(), "OnlyOne") }.AsQueryable());

        // Act
        Result<ArticleInformation> result = await useCase.AddTags(input);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NotFoundError>();
    }

    [Fact]
    public async Task AddTags_ShouldAddTags_WhenInputsAreValid()
    {
        // Arrange
        Article article = CreateArticle();
        Guid tagId1 = Guid.NewGuid();
        Guid tagId2 = Guid.NewGuid();
        Tag tag1 = CreateTag(tagId1, "Tag1");
        Tag tag2 = CreateTag(tagId2, "Tag2");

        var input = new AddTagsToArticle(
            articleId: article.Id,
            tagIds: new[] { tagId1, tagId2 });

        articleRepositoryMock.Setup(pre => pre.GetById(article.Id))
            .ReturnsAsync(article);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(new List<Tag> { tag1, tag2 }.AsQueryable());

        articleRepositoryMock.Setup(pre => pre.Update(It.IsAny<Article>()))
            .Returns((Article a) => a);

        // Act
        Result<ArticleInformation> result = await useCase.AddTags(input);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        article.Tags.Count.ShouldBe(2);
        articleRepositoryMock.Verify(pre => pre.Update(It.IsAny<Article>()), Times.Once());
    }

    [Fact]
    public async Task AddTags_ShouldFail_WhenTagLimitExceeded()
    {
        // Arrange
        List<Tag> existingTags = Enumerable.Range(0, 20)
            .Select(i => CreateTag(Guid.NewGuid(), $"Tag{i}"))
            .ToList();
        Article article = CreateArticle(tags: existingTags);

        Guid newTagId = Guid.NewGuid();
        var input = new AddTagsToArticle(
            articleId: article.Id,
            tagIds: new[] { newTagId });

        articleRepositoryMock.Setup(pre => pre.GetById(article.Id))
            .ReturnsAsync(article);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(new List<Tag> { CreateTag(newTagId, "NewTag") }.AsQueryable());

        // Act
        Result<ArticleInformation> result = await useCase.AddTags(input);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<DomainError>();
    }

    // ─── RemoveTags ───────────────────────────────────────────────

    [Fact]
    public async Task RemoveTags_ShouldFail_WhenArticleNotFound()
    {
        // Arrange
        var input = new RemoveTagsToArticle(
            articleId: Guid.NewGuid(),
            tagIds: new[] { Guid.NewGuid() });

        articleRepositoryMock.Setup(pre => pre.GetById(It.IsAny<Guid>()))
            .ReturnsAsync((Article?)null);

        // Act
        Result<ArticleInformation> result = await useCase.RemoveTags(input);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NotFoundError>();
    }

    [Fact]
    public async Task RemoveTags_ShouldFail_WhenTagsNotFound()
    {
        // Arrange
        Article article = CreateArticle();
        Guid[] tagIds = { Guid.NewGuid(), Guid.NewGuid() };

        var input = new RemoveTagsToArticle(
            articleId: article.Id,
            tagIds: tagIds);

        articleRepositoryMock.Setup(pre => pre.GetById(article.Id))
            .ReturnsAsync(article);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(new List<Tag> { CreateTag(Guid.NewGuid(), "OnlyOne") }.AsQueryable());

        // Act
        Result<ArticleInformation> result = await useCase.RemoveTags(input);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NotFoundError>();
    }

    [Fact]
    public async Task RemoveTags_ShouldRemoveTags_WhenInputsAreValid()
    {
        // Arrange
        Guid tagId1 = Guid.NewGuid();
        Guid tagId2 = Guid.NewGuid();
        Tag tag1 = CreateTag(tagId1, "Tag1");
        Tag tag2 = CreateTag(tagId2, "Tag2");

        Article article = CreateArticle(tags: new List<Tag> { tag1, tag2 });

        var input = new RemoveTagsToArticle(
            articleId: article.Id,
            tagIds: new[] { tagId1 });

        articleRepositoryMock.Setup(pre => pre.GetById(article.Id))
            .ReturnsAsync(article);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(new List<Tag> { tag1 }.AsQueryable());

        articleRepositoryMock.Setup(pre => pre.Update(It.IsAny<Article>()))
            .Returns((Article a) => a);

        // Act
        Result<ArticleInformation> result = await useCase.RemoveTags(input);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        article.Tags.Count.ShouldBe(1);
        article.Tags[0].Id.ShouldBe(tagId2);
        articleRepositoryMock.Verify(pre => pre.Update(It.IsAny<Article>()), Times.Once());
    }

    [Fact]
    public async Task RemoveTags_ShouldNotFail_WhenTagsNotInArticle()
    {
        // Arrange
        Guid tagId1 = Guid.NewGuid();
        Guid tagId2 = Guid.NewGuid();
        Tag tag1 = CreateTag(tagId1, "Tag1");
        Tag tag2 = CreateTag(tagId2, "Tag2");

        Article article = CreateArticle(tags: new List<Tag> { tag1 });

        var input = new RemoveTagsToArticle(
            articleId: article.Id,
            tagIds: new[] { tagId2 });

        articleRepositoryMock.Setup(pre => pre.GetById(article.Id))
            .ReturnsAsync(article);

        tagRepositoryMock.Setup(pre => pre.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(new List<Tag> { tag2 }.AsQueryable());

        articleRepositoryMock.Setup(pre => pre.Update(It.IsAny<Article>()))
            .Returns((Article a) => a);

        // Act
        Result<ArticleInformation> result = await useCase.RemoveTags(input);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        article.Tags.Count.ShouldBe(1);
        article.Tags[0].Id.ShouldBe(tagId1);
    }

    // ─── FindByFilter ─────────────────────────────────────────────

    [Fact]
    public void FindByFilter_ShouldCallGetAll_WhenNoFilters()
    {
        // Arrange
        var filters = new ArticleFilters(Title: null, TagNames: null);
        articleRepositoryMock.Setup(pre => pre.GetAll())
            .Returns(new List<Article>().AsQueryable());

        // Act
        var result = useCase.FindByFilter(filters);

        // Assert
        result.ShouldNotBeNull();
        articleRepositoryMock.Verify(pre => pre.GetAll(), Times.Once());
        articleRepositoryMock.Verify(pre => pre.Filter(It.IsAny<string?>(), It.IsAny<List<string>?>()), Times.Never());
    }

    [Fact]
    public void FindByFilter_ShouldCallFilter_WhenTitleProvided()
    {
        // Arrange
        string title = faker.Random.Word();
        var filters = new ArticleFilters(Title: title, TagNames: null);
        articleRepositoryMock.Setup(pre => pre.Filter(title, null))
            .Returns(new List<Article>().AsQueryable());

        // Act
        var result = useCase.FindByFilter(filters);

        // Assert
        result.ShouldNotBeNull();
        articleRepositoryMock.Verify(pre => pre.Filter(title, null), Times.Once());
        articleRepositoryMock.Verify(pre => pre.GetAll(), Times.Never());
    }

    [Fact]
    public void FindByFilter_ShouldCallFilter_WhenTagNamesProvided()
    {
        // Arrange
        List<string> tagNames = new() { "csharp", "dotnet" };
        var filters = new ArticleFilters(Title: null, TagNames: tagNames);
        articleRepositoryMock.Setup(pre => pre.Filter(null, tagNames))
            .Returns(new List<Article>().AsQueryable());

        // Act
        var result = useCase.FindByFilter(filters);

        // Assert
        result.ShouldNotBeNull();
        articleRepositoryMock.Verify(pre => pre.Filter(null, tagNames), Times.Once());
        articleRepositoryMock.Verify(pre => pre.GetAll(), Times.Never());
    }

    [Fact]
    public void FindByFilter_ShouldCallFilter_WhenBothTitleAndTagNamesProvided()
    {
        // Arrange
        string title = faker.Random.Word();
        List<string> tagNames = new() { "csharp" };
        var filters = new ArticleFilters(Title: title, TagNames: tagNames);
        articleRepositoryMock.Setup(pre => pre.Filter(title, tagNames))
            .Returns(new List<Article>().AsQueryable());

        // Act
        var result = useCase.FindByFilter(filters);

        // Assert
        result.ShouldNotBeNull();
        articleRepositoryMock.Verify(pre => pre.Filter(title, tagNames), Times.Once());
        articleRepositoryMock.Verify(pre => pre.GetAll(), Times.Never());
    }

    // ─── SetOrUnsetFavoriteArticle ────────────────────────────────

    [Fact]
    public async Task SetOrUnsetFavoriteArticle_ShouldFail_WhenArticleNotFound()
    {
        // Arrange
        var token = new CancellationToken();
        Guid articleId = Guid.NewGuid();
        var input = new ArticleById(articleId);

        articleRepositoryMock.Setup(pre => pre.GetById(articleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article?)null);

        // Act
        Result<ArticleInformation> result = await useCase.SetOrUnsetFavoriteArticle(input, token);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NotFoundError>();
    }

    [Fact]
    public async Task SetOrUnsetFavoriteArticle_ShouldToggleFavorite_WhenArticleExists()
    {
        // Arrange
        Article article = CreateArticle();
        var token = new CancellationToken();
        article.IsFavorite = false;
        var input = new ArticleById(article.Id);

        articleRepositoryMock.Setup(pre => pre.GetById(article.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        articleRepositoryMock.Setup(pre => pre.Update(It.IsAny<Article>()))
            .Returns((Article a) => a);

        // Act
        Result<ArticleInformation> result = await useCase.SetOrUnsetFavoriteArticle(input, token);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.IsFavorite.ShouldBeTrue();
        article.IsFavorite.ShouldBeTrue();
        articleRepositoryMock.Verify(pre => pre.Update(It.IsAny<Article>()), Times.Once());
    }

    [Fact]
    public async Task SetOrUnsetFavoriteArticle_ShouldToggleBackToFalse_WhenCalledTwice()
    {
        // Arrange
        var token = new CancellationToken();
        Article article = CreateArticle();
        article.IsFavorite = true;
        var input = new ArticleById(article.Id);

        articleRepositoryMock.Setup(pre => pre.GetById(article.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        articleRepositoryMock.Setup(pre => pre.Update(It.IsAny<Article>()))
            .Returns((Article a) => a);

        // Act
        Result<ArticleInformation> result = await useCase.SetOrUnsetFavoriteArticle(input, token);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.IsFavorite.ShouldBeFalse();
        article.IsFavorite.ShouldBeFalse();
    }
}
