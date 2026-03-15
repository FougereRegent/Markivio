using Markivio.Persistence;
using Markivio.Persistence.Config;
using Markivio.UnitTests.Helper;
using Microsoft.EntityFrameworkCore.Storage;
using Shouldly;
using Moq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Markivio.Domain.Auth;
using Markivio.Domain.Entities;
using Markivio.Domain.ValueObject;

namespace Markivio.UnitTests.Infra;

public class UnitOfWorkTests
{
    private Mock<MarkivioContext> dbContextMock;
    private Mock<IDbContextTransaction> dbContextTransactionMock;
    private Mock<DatabaseFacade> databaseFacadeMock;
    private Mock<IAuthUser> authUserMock;

    public UnitOfWorkTests()
    {
        authUserMock = new Mock<IAuthUser>(MockBehavior.Loose);
        authUserMock.SetupProperty(pre => pre.CurrentUser, CreateValidUser());

        // Moq needs a real constructor; we never use EF provider behavior in these tests.
        DbContextOptions<MarkivioContext> options = new DbContextOptionsBuilder<MarkivioContext>().Options;
        dbContextMock = new Mock<MarkivioContext>(options, authUserMock.Object) { };
        databaseFacadeMock = new Mock<DatabaseFacade>(dbContextMock.Object);
        dbContextMock.Setup(pre => pre.Database)
          .Returns(databaseFacadeMock.Object);
        dbContextTransactionMock = new Mock<IDbContextTransaction>();
    }

    private static User CreateValidUser()
    {
        var identity = new IdentityValueObject("u", "John", "Doe");
        var email = new EmailValueObject("john.doe@example.com");
        return new User(identity, email) { Id = Guid.NewGuid(), AuthId = "auth0|test" };
    }

    [Fact]
    public async Task BeginTransactionAsyncTest_Should_CreateTransaction()
    {
        //Arrange
        UnitOfWork unitOfWork = new UnitOfWork(dbContextMock.Object);
        databaseFacadeMock.Setup(pre => pre.BeginTransactionAsync(It.IsAny<CancellationToken>()))
          .Returns(Task.FromResult(dbContextTransactionMock.Object));

        //Act
        await unitOfWork.BeginTransactionAsync();

        //Assert
        IDbContextTransaction result = unitOfWork.GetPrivateField<UnitOfWork, IDbContextTransaction>("transaction")!;
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task RollbackChangesAsync_Should_Rollback()
    {
        //Arrange
        UnitOfWork unitOfWork = new UnitOfWork(dbContextMock.Object);
        databaseFacadeMock.Setup(pre => pre.BeginTransactionAsync(It.IsAny<CancellationToken>()))
          .Returns(Task.FromResult(dbContextTransactionMock.Object));
        dbContextTransactionMock.Setup(pre => pre.RollbackAsync(It.IsAny<CancellationToken>()))
          .Returns(Task.CompletedTask);

        await unitOfWork.BeginTransactionAsync();

        //Act & Assert
        await Should.NotThrowAsync(async () => await unitOfWork.RollbackChangesAsync());
    }

    [Fact]
    public async Task RollbackChangesAsync_Should_ThrowError_When_TransactionIsNull()
    {
        UnitOfWork unitOfWork = new UnitOfWork(dbContextMock.Object);
        dbContextTransactionMock.Setup(pre => pre.RollbackAsync(It.IsAny<CancellationToken>()))
          .Returns(Task.CompletedTask);

        //Act & Assert
        await Should.ThrowAsync<InvalidOperationException>(async () => await unitOfWork.RollbackChangesAsync());

    }

    [Fact]
    public async Task SaveChangesAsync_Should_Rollback()
    {
        //Arrange
        UnitOfWork unitOfWork = new UnitOfWork(dbContextMock.Object);
        databaseFacadeMock.Setup(pre => pre.BeginTransactionAsync(It.IsAny<CancellationToken>()))
          .Returns(Task.FromResult(dbContextTransactionMock.Object));
        dbContextTransactionMock.Setup(pre => pre.CommitAsync(It.IsAny<CancellationToken>()))
          .Returns(Task.CompletedTask);

        await unitOfWork.BeginTransactionAsync();

        //Act & Assert
        await Should.NotThrowAsync(async () => await unitOfWork.SaveChangesAsync());
    }

    [Fact]
    public async Task SaveChangesAsync_Should_ThrowError_When_TransactionIsNull()
    {
        UnitOfWork unitOfWork = new UnitOfWork(dbContextMock.Object);
        dbContextTransactionMock.Setup(pre => pre.RollbackAsync(It.IsAny<CancellationToken>()))
          .Returns(Task.CompletedTask);

        //Act & Assert
        await Should.ThrowAsync<InvalidOperationException>(async () => await unitOfWork.SaveChangesAsync());
    }
}
