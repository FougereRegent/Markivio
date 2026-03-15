using Markivio.Application.UseCases;
using Markivio.Domain.Auth;
using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Domain.ValueObject;
using Moq;

namespace Markivio.UnitTests.Application;


public abstract class BaseTagTests : BaseTests
{
    protected Mock<ITagRepository> tagRepositoryMock;
    protected Mock<IAuthUser> authUserMock;

    protected TagUseCase useCase;

    public BaseTagTests()
    {
        tagRepositoryMock = new Mock<ITagRepository>();
        authUserMock = new Mock<IAuthUser>();

        useCase = new TagUseCase(tagRepositoryMock.Object, authUserMock.Object);
    }

    protected User CreateValidUser()
    {
        var identity = new IdentityValueObject(
            userName: faker.Internet.UserName(),
            firstName: faker.Person.FirstName,
            lastName: faker.Person.LastName);
        var email = new EmailValueObject(faker.Internet.Email());

        return new User(identity, email) { Id = Guid.NewGuid(), AuthId = faker.Random.Guid().ToString() };
    }
}
