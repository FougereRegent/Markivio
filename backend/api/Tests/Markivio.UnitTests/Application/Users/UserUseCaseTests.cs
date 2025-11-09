using Bogus;
using FluentResults;
using Markivio.Application.Dto;
using Markivio.Application.Errors;
using Markivio.Application.UseCases;
using Markivio.Domain.Auth;
using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Moq;
using Shouldly;

namespace Markivio.UnitTests.Application.Users;

public class UserUseCaseTests
{
    public Mock<IUserRepository> userRepositoryMock;
    public Mock<IAuthUser> authUserMock;
    public UserUseCase useCase;

    public UserUseCaseTests()
    {
        this.userRepositoryMock = new Mock<IUserRepository>();
        this.authUserMock = new Mock<IAuthUser>();
        this.useCase = new UserUseCase(this.userRepositoryMock.Object, this.authUserMock.Object);
    }

    [Fact]
    public async ValueTask CreateNewUserOnConnection_ShouldReturnFailedResult_WhenTokenIsCorrupted()
    {
        //Arrange
        authUserMock.Setup(pre => pre.GetUserInfoByToken(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(null));

        //Act
        Result result = await useCase.CreateNewUserOnConnection(new Markivio.Application.Dto.UserConnectionDto(""));

        //Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBe("User not found");
    }

    [Fact]
    public async ValueTask CreateNewUserOnConnection_ShouldNotSaveUser_WhenUserExist()
    {
        //Arrange
        authUserMock.Setup(pre => pre.GetUserInfoByToken(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(new User()));
        userRepositoryMock.Setup(pre => pre.GetUserByAuthId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(new User()));
        userRepositoryMock.Setup(pre => pre.Save(It.IsAny<User>()));

        //Act
        Result result = await useCase.CreateNewUserOnConnection(new Markivio.Application.Dto.UserConnectionDto(""));

        //Assert
        userRepositoryMock.Verify(pre => pre.Save(It.IsAny<User>()), Times.Never());
        result.IsSuccess.ShouldBeTrue();

    }

    [Fact]
    public async ValueTask CreateNewUserOnConnection_ShouldSave_WhenUserNotExist()
    {
        //Arrange
        authUserMock.Setup(pre => pre.GetUserInfoByToken(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(new User()));
        userRepositoryMock.Setup(pre => pre.GetUserByAuthId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(null));
        userRepositoryMock.Setup(pre => pre.Save(It.IsAny<User>()));

        //Act
        Result result = await useCase.CreateNewUserOnConnection(new Markivio.Application.Dto.UserConnectionDto(""));

        //Assert
        userRepositoryMock.Verify(pre => pre.Save(It.IsAny<User>()), Times.Once());
        result.IsSuccess.ShouldBeTrue();

    }


    [Fact]
    public async ValueTask UpdateCurrentUser_ShouldNotUpdate_WhenCurrentUserIsNotFound()
    {
        //Arrange
        authUserMock
          .Setup(pre => pre.CurrentUser)
          .Returns(new User
          {
              Id = Guid.NewGuid()
          });
        userRepositoryMock
          .Setup(pre => pre.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(null));

        //Act
        Result<UserInformation> result = await useCase.UpdateCurrentUser(new UpdateUserInformation());

        //Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBe("Cannot found");
        result.Errors[0].GetType().ShouldBe(typeof(NotFoundError));
    }


    public static IEnumerable<object[]> GetPersonUserName()
    {
        for (int i = 0; i < 10; ++i)
        {
            Faker faker = new Faker("fr");
            yield return new object[] { faker.Person.FirstName, faker.Person.LastName };
        }
    }

    [Theory]
    [MemberData(nameof(GetPersonUserName))]
    public async ValueTask UpdateCurrentUser_ShouldUpdate(string firstName,
        string lastName)
    {
        //Arrange
        Faker faker = new Faker("fr");
        User currentUser = new User()
        {
            Id = Guid.NewGuid(),
            Email = faker.Internet.Email(),
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName
        };
        authUserMock
          .Setup(pre => pre.CurrentUser)
          .Returns(currentUser);

        userRepositoryMock
          .Setup(pre => pre.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(currentUser));

        userRepositoryMock
          .Setup(pre => pre.Update(It.IsAny<User>()))
          .Returns(currentUser);

        //Act
        Result<UserInformation> result = await useCase.UpdateCurrentUser(new UpdateUserInformation(firstName, lastName));

        //Assert
        result.IsSuccess.ShouldBeTrue();
        UserInformation user = result.Value;
        user.FirstName.ShouldBe(firstName);
        user.LastName.ShouldBe(lastName);
    }
}
