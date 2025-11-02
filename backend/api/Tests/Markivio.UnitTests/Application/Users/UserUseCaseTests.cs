using Bogus;
using FluentResults;
using Markivio.Application.Dto;
using Markivio.Application.Errors;
using Markivio.Application.UseCases;
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
        Result result = await useCase.CreateNewUserOnConnection(new Markivio.Application.Dto.UserConnectionDto(""));

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
        Result result = await useCase.CreateNewUserOnConnection(new Markivio.Application.Dto.UserConnectionDto(""));

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
        Result result = await useCase.CreateNewUserOnConnection(new Markivio.Application.Dto.UserConnectionDto(""));

        //Assert
        userRepositoryMock.Verify(pre => pre.Save(It.IsAny<User>()), Times.Once());
        result.IsSuccess.ShouldBeTrue();

    }

    [Fact]
    public async ValueTask Me_ShouldReturnFailedResult_WhenUserNotExist()
    {
        //Arrange
        const string jwtToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkpQZzA0RXJvajJGVEg1MmVURWJqTCJ9.eyJpc3MiOiJodHRwczovL21hcmtpdmlvLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJnb29nbGUtb2F1dGgyfDExMjExMDYzMTEwNDcxNDAyMTg3NyIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo4MDgwLyIsImh0dHBzOi8vbWFya2l2aW8uZXUuYXV0aDAuY29tL3VzZXJpbmZvIl0sImlhdCI6MTc2MTU5OTIwNCwiZXhwIjoxNzYxNjg1NjA0LCJzY29wZSI6Im9wZW5pZCBwcm9maWxlIGVtYWlsIiwiYXpwIjoiVFdsYlZBZ25Pem5FVVJHekZidkFydTE1aUdZMkNiQVcifQ.NJ9jSw0rKCNCVWvwl0No9WMf0cbbGyIvX_lC3LOus0l2-qtw2sY-xDwsISY5BnZhbyt-_0PA4PqwPdBoUaoJtdvbJG6LEKHMkwlYT9qo92cYLxFeY0w5Ev8aMjouuSAa5n2lDyK3Q6w18KZbRw6T_rTjRQu3ULriIemCTt-SQyJrmVaqb4RgQVgkrF09NTH3tSTgxVwFf0P-ey_PH7fVRWg3Bc-JJew6E6jJ1qWlQe358M1w_uCVdY5av9KY1rng3Ud-hokBnivID9tdf9z2eOt2cGyjoQ5lcaBi7U6jw-2l326ikDqrjQZm5wRtgWCAlzl9SUclq7V0QKGyCrw1gQ";
        userRepositoryMock.Setup(pre => pre.GetUserByAuthId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(null));

        //Act
        Result<UserInformation> result = await useCase.Me(new Markivio.Application.Dto.UserConnectionDto(jwtToken));

        //Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBe("User not found");
    }

    [Fact]
    public async ValueTask Me_ShouldReturnUser()
    {
        //Arrange
        const string jwtToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkpQZzA0RXJvajJGVEg1MmVURWJqTCJ9.eyJpc3MiOiJodHRwczovL21hcmtpdmlvLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJnb29nbGUtb2F1dGgyfDExMjExMDYzMTEwNDcxNDAyMTg3NyIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo4MDgwLyIsImh0dHBzOi8vbWFya2l2aW8uZXUuYXV0aDAuY29tL3VzZXJpbmZvIl0sImlhdCI6MTc2MTU5OTIwNCwiZXhwIjoxNzYxNjg1NjA0LCJzY29wZSI6Im9wZW5pZCBwcm9maWxlIGVtYWlsIiwiYXpwIjoiVFdsYlZBZ25Pem5FVVJHekZidkFydTE1aUdZMkNiQVcifQ.NJ9jSw0rKCNCVWvwl0No9WMf0cbbGyIvX_lC3LOus0l2-qtw2sY-xDwsISY5BnZhbyt-_0PA4PqwPdBoUaoJtdvbJG6LEKHMkwlYT9qo92cYLxFeY0w5Ev8aMjouuSAa5n2lDyK3Q6w18KZbRw6T_rTjRQu3ULriIemCTt-SQyJrmVaqb4RgQVgkrF09NTH3tSTgxVwFf0P-ey_PH7fVRWg3Bc-JJew6E6jJ1qWlQe358M1w_uCVdY5av9KY1rng3Ud-hokBnivID9tdf9z2eOt2cGyjoQ5lcaBi7U6jw-2l326ikDqrjQZm5wRtgWCAlzl9SUclq7V0QKGyCrw1gQ";
        userRepositoryMock.Setup(pre => pre.GetUserByAuthId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .Returns(ValueTask.FromResult<User?>(new User()));

        //Act
        Result<UserInformation> result = await useCase.Me(new Markivio.Application.Dto.UserConnectionDto(jwtToken));

        //Assert
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async ValueTask UpdateCurrentUser_ShouldNotUpdate_WhenCurrentUserIsNotFound()
    {
        //Arrange
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
