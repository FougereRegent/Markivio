
using Bogus;
using Markivio.Application.Dto;
using Markivio.Application.Errors;
using Markivio.Application.UseCases;
using Markivio.Domain.Auth;
using Markivio.Domain.Entities;
using Markivio.Domain.Errors;
using Markivio.Domain.Repositories;
using Moq;
using Shouldly;

namespace Markivio.UnitTests.Application;

public class ArticleTests
{
    private Mock<IAuthUser> authUserMock;
    private Mock<ITagUseCase> tagUseCaseMock;
    private Mock<IArticleRepository> articleRepositoryMock;
    private Mock<ITagRepository> tagRepositporyMock;

    private ArticleUseCase articleUseCase;

    public ArticleTests()
    {
        authUserMock = new Mock<IAuthUser>();
        tagUseCaseMock = new Mock<ITagUseCase>();
        articleRepositoryMock = new Mock<IArticleRepository>();
        tagRepositporyMock = new Mock<ITagRepository>();
        articleUseCase = new ArticleUseCase(tagUseCaseMock.Object, articleRepositoryMock.Object, tagRepositporyMock.Object, authUserMock.Object);
    }

    [Fact]
    public async Task CreateArticle_ShouldntCreate_WhenArticleAlreadyExist()
    {
        //Arrange
        Article data = new Article
        {
            Id = Guid.NewGuid()
        };
        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(ValueTask.FromResult<Article?>(data));

        //Act 
        FluentResults.Result<ArticleInformation> result = await articleUseCase.CreateArticle(new CreateArticle(default, default, default, new TagCreateArticle[0]));

        //Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<AlreadyExistError>();
    }

    [Fact]
    public async Task CreateArticle_ShouldntCreate_WhenArticleTagsNotExist()
    {
        //Arrange
        Faker faker = new Faker("fr");
        CreateArticle inputData = new CreateArticle(faker.Random.Word(), faker.Internet.Url(), faker.Lorem.Sentence(), new TagCreateArticle[] {
            new TagCreateArticle(Guid.NewGuid()),
            new TagCreateArticle(Guid.NewGuid()),
            new TagCreateArticle(Guid.NewGuid()),
            new TagCreateArticle(Guid.NewGuid()),
        });
        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(ValueTask.FromResult<Article?>(null));
        tagUseCaseMock.Setup(pre => pre.TagsExist(It.IsAny<Tag[]>()))
          .Returns(false);


        //Act 
        FluentResults.Result<ArticleInformation> result = await articleUseCase.CreateArticle(inputData);

        //Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NotFoundError>();
    }

    [Fact]
    public async Task CreateArticle_ShouldntCreate_WhenArticleIsNotValidate()
    {
        //Arrange
        Faker faker = new Faker("fr");
        CreateArticle inputData = new CreateArticle(faker.Random.Word(), "", faker.Lorem.Sentence(), new TagCreateArticle[] {
            new TagCreateArticle(Guid.NewGuid()),
            new TagCreateArticle(Guid.NewGuid()),
            new TagCreateArticle(Guid.NewGuid()),
        });
        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(ValueTask.FromResult<Article?>(null));
        tagUseCaseMock.Setup(pre => pre.TagsExist(It.IsAny<Tag[]>()))
          .Returns(true);
        authUserMock.Setup(pre => pre.CurrentUser)
          .Returns(new User());

        //Act 
        FluentResults.Result<ArticleInformation> result = await articleUseCase.CreateArticle(inputData);

        //Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<FormatUnexpectedError>();
    }

    [Fact]
    public async Task CreateArticle_ShouldntCreate_WhenArticleIsNotValidateWhithoutUser()
    {
        //Arrange
        Faker faker = new Faker("fr");
        CreateArticle inputData = new CreateArticle(faker.Random.Word(), faker.Internet.Url(), faker.Lorem.Sentence(), new TagCreateArticle[] {
            new TagCreateArticle(Guid.NewGuid()),
            new TagCreateArticle(Guid.NewGuid()),
            new TagCreateArticle(Guid.NewGuid()),
        });
        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(ValueTask.FromResult<Article?>(null));
        tagUseCaseMock.Setup(pre => pre.TagsExist(It.IsAny<Tag[]>()))
          .Returns(true);

        //Act 
        FluentResults.Result<ArticleInformation> result = await articleUseCase.CreateArticle(inputData);

        //Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NullFieldError>();
    }

    [Fact]
    public async Task CreateArticle_Test()
    {
        //Arrange
        Faker faker = new Faker("fr");
        string title = faker.Random.Word();
        string url = faker.Internet.Url();
        CreateArticle inputData = new CreateArticle(title, url, faker.Lorem.Sentence(), new TagCreateArticle[] {
            new TagCreateArticle(Guid.NewGuid()),
            new TagCreateArticle(Guid.NewGuid()),
            new TagCreateArticle(Guid.NewGuid()),
        });
        articleRepositoryMock.Setup(obj => obj.GetByTitle(It.IsAny<string>()))
          .Returns(ValueTask.FromResult<Article?>(null));
        tagUseCaseMock.Setup(pre => pre.TagsExist(It.IsAny<Tag[]>()))
          .Returns(true);
        authUserMock.Setup(pre => pre.CurrentUser)
          .Returns(new User());
        articleRepositoryMock.Setup(pre => pre.Save(It.IsAny<Article>()))
          .Returns(new Article
          {
              Id = Guid.NewGuid(),
              Title = title,
              User = new User(),
              ArticleContent = new ArticleContent
              {
                  Source = url
              }
          });

        //Act 
        FluentResults.Result<ArticleInformation> result = await articleUseCase.CreateArticle(inputData);

        //Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldBeOfType<ArticleInformation>();
    }
}
