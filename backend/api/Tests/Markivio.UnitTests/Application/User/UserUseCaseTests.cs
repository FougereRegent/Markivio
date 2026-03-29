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

public sealed class UserUseCaseTests : BaseTests
{
    private readonly Mock<IUserRepository> userRepositoryMock = new();
    private readonly Mock<IAuthUser> authUserMock = new();
    private readonly UserUseCase useCase;

    public UserUseCaseTests()
    {
        useCase = new UserUseCase(userRepositoryMock.Object, authUserMock.Object);
    }

    private User CreateValidUser(string? authId = null)
    {
        var identity = new IdentityValueObject(
            userName: faker.Internet.UserName(),
            firstName: faker.Person.FirstName,
            lastName: faker.Person.LastName);
        var email = new EmailValueObject(faker.Internet.Email());
        return new User(identity, email) { Id = Guid.NewGuid(), AuthId = authId ?? faker.Random.Guid().ToString() };
    }

    [Fact]
    public async ValueTask CreateNewUserOnConnection_ShouldReturnFailedResult_WhenTokenIsCorrupted()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        authUserMock.Setup(pre => pre.GetUserInfoByToken(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(null));

        // Act
        Result result = await useCase.CreateNewUserOnConnection(new UserConnectionDto(""), token);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NotFoundError>();
        result.Errors[0].Message.ShouldBe("User not found");
    }

    [Fact]
    public async ValueTask CreateNewUserOnConnection_ShouldNotSaveUser_WhenUserExist()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        User userFromToken = CreateValidUser(authId: "auth0|token");
        User userFromDb = CreateValidUser(authId: "auth0|token");

        authUserMock.Setup(pre => pre.GetUserInfoByToken(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(userFromToken));

        userRepositoryMock.Setup(pre => pre.GetUserByAuthId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(userFromDb));

        // Act
        Result result = await useCase.CreateNewUserOnConnection(new UserConnectionDto("token"), token);

        // Assert
        userRepositoryMock.Verify(pre => pre.Save(It.IsAny<User>()), Times.Never());
        result.IsSuccess.ShouldBeTrue();
        authUserMock.VerifySet(pre => pre.CurrentUser = userFromDb, Times.Once());
    }

    [Fact]
    public async ValueTask CreateNewUserOnConnection_ShouldSave_WhenUserNotExist()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        User userFromToken = CreateValidUser(authId: "auth0|token");

        authUserMock.Setup(pre => pre.GetUserInfoByToken(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(userFromToken));

        userRepositoryMock.Setup(pre => pre.GetUserByAuthId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(null));

        // Act
        Result result = await useCase.CreateNewUserOnConnection(new UserConnectionDto("token"), token);

        // Assert
        userRepositoryMock.Verify(pre => pre.Save(It.IsAny<User>()), Times.Once());
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async ValueTask UpdateCurrentUser_ShouldNotUpdate_WhenCurrentUserIsNotFound()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        User currentUser = CreateValidUser(authId: "auth0|current");
        authUserMock.Setup(pre => pre.CurrentUser).Returns(currentUser);

        userRepositoryMock
          .Setup(pre => pre.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(null));

        // Act
        Result<UserInformation> result = await useCase.UpdateCurrentUser(new UpdateUserInformation("John", "Doe"), token);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<NotFoundError>();
        result.Errors[0].Message.ShouldBe("Cannot found");
    }

    [Fact]
    public async ValueTask UpdateCurrentUser_ShouldFail_WhenUpdateIsInvalid()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        User currentUser = CreateValidUser(authId: "auth0|current");
        authUserMock.Setup(pre => pre.CurrentUser).Returns(currentUser);

        userRepositoryMock
          .Setup(pre => pre.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(currentUser));

        // Act
        Result<UserInformation> result = await useCase.UpdateCurrentUser(new UpdateUserInformation("&^^", "Doe"), token);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<DomainError>();
        result.Errors[0].Metadata[ErrorCode.ERROR_CODE_PROPERTY_NAME].ShouldBe("FORMAT_FIRSTNAME");
        userRepositoryMock.Verify(pre => pre.Update(It.IsAny<User>()), Times.Never());
    }

    [Fact]
    public async ValueTask UpdateCurrentUser_ShouldUpdate_WhenInputsAreValid()
    {
        // Arrange
        CancellationToken token = new CancellationToken();
        Faker localFaker = new Faker("fr");
        User currentUser = CreateValidUser(authId: "auth0|current");
        authUserMock.Setup(pre => pre.CurrentUser).Returns(currentUser);

        userRepositoryMock
          .Setup(pre => pre.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(currentUser));

        userRepositoryMock
          .Setup(pre => pre.Update(It.IsAny<User>()))
          .Returns((User u) => u);

        string firstName = localFaker.Person.FirstName;
        string lastName = localFaker.Person.LastName;

        // Act
        Result<UserInformation> result = await useCase.UpdateCurrentUser(new UpdateUserInformation(firstName, lastName), token);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.FirstName.ShouldBe(firstName);
        result.Value.LastName.ShouldBe(lastName);
        userRepositoryMock.Verify(pre => pre.Update(It.IsAny<User>()), Times.Once());
    }
}
