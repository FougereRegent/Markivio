using Markivio.Application.UseCases;
using Markivio.Domain.Auth;
using Markivio.Domain.Repositories;
using Moq;

namespace Markivio.UnitTests.Application;


public abstract class BaseTagTests
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
}
