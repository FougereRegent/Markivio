using Markivio.Application.Users;
using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Moq;
using Shouldly;

namespace Markivio.UnitTests.Application.Users;

public class UserUseCaseTests
{
    public Mock<IUserRepository> userRepositoryMock;
    public UserUseCase useCase;

    public UserUseCaseTests()
    {
        this.userRepositoryMock = new Mock<IUserRepository>();
        this.useCase = new UserUseCase(this.userRepositoryMock.Object);
    }

    [Fact]
    public async ValueTask CreateNewUserOnConnection_ShouldReturnFailedResult_WhenTokenIsCorrupted()
    {
        //Arrange
        userRepositoryMock.Setup(pre => pre.GetUserInfoByToken(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(null));

        //Act
        FluentResults.Result result = await useCase.CreateNewUserOnConnection(new Markivio.Application.Dto.UserConnectionDto(""));

        //Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBe("User not found");
    }

    [Fact]
    public async ValueTask CreateNewUserOnConnection_ShouldNotSaveUser_WhenUserExist()
    {
        //Arrange
        userRepositoryMock.Setup(pre => pre.GetUserInfoByToken(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(new User()));
        userRepositoryMock.Setup(pre => pre.GetUserByAuthId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(new User()));
        userRepositoryMock.Setup(pre => pre.Save(It.IsAny<User>()));

        //Act
        FluentResults.Result result = await useCase.CreateNewUserOnConnection(new Markivio.Application.Dto.UserConnectionDto(""));

        //Assert
        userRepositoryMock.Verify(pre => pre.Save(It.IsAny<User>()), Times.Never());
        result.IsSuccess.ShouldBeTrue();

    }

    [Fact]
    public async ValueTask CreateNewUserOnConnection_ShouldSave_WhenUserNotExist()
    {
        //Arrange
        userRepositoryMock.Setup(pre => pre.GetUserInfoByToken(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(new User()));
        userRepositoryMock.Setup(pre => pre.GetUserByAuthId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(null));
        userRepositoryMock.Setup(pre => pre.Save(It.IsAny<User>()));

        //Act
        FluentResults.Result result = await useCase.CreateNewUserOnConnection(new Markivio.Application.Dto.UserConnectionDto(""));

        //Assert
        userRepositoryMock.Verify(pre => pre.Save(It.IsAny<User>()), Times.Once());
        result.IsSuccess.ShouldBeTrue();

    }
}
