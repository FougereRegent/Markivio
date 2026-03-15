
using FluentResults;
using Markivio.Application.Dto;
using Markivio.Application.Errors;
using Markivio.Domain.Entities;
using Markivio.Domain.Errors;
using Moq;
using Shouldly;

namespace Markivio.UnitTests.Application;

public class TagCreationTests : BaseTagTests
{
    [Fact]
    public void CreateTag_ShouldntCreate_WhenThereAreDuplicateTag()
    {
        //Arrange
        CreateTag[] input = new CreateTag[] {
          new CreateTag("Name", "#FFFFFF"),
          new CreateTag("Name", "#FFFFFF"),
        };
        tagRepositoryMock.Setup(pre => pre.GetAll())
          .Returns(Enumerable.Empty<Tag>().AsQueryable());
        authUserMock.Setup(pre => pre.CurrentUser)
          .Returns(CreateValidUser());

        // Act
        Result<TagInformation[]> result = useCase.CreateTag(input);

        //Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<DuplicatedItemsError>();
    }

    [Fact]
    public void CreateTag_ShouldntCreate_WhenThereAreValidationError()
    {
        //Arrange
        CreateTag[] input = new CreateTag[] {
          new CreateTag("", "#FFFFFF"),
          new CreateTag("Test", "#FFFFFF"),
        };
        tagRepositoryMock.Setup(pre => pre.GetAll())
          .Returns(Enumerable.Empty<Tag>().AsQueryable());
        authUserMock.Setup(pre => pre.CurrentUser)
          .Returns(CreateValidUser());

        // Act
        Result<TagInformation[]> result = useCase.CreateTag(input);

        //Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<ShouldNotBeEmptyError>();
        tagRepositoryMock.Verify(pre => pre.SaveInRange(It.IsAny<IEnumerable<Tag>>()), Times.Never());
    }

    [Fact]
    public void CreateTag_Should_Create()
    {
        //Arrange
        CreateTag[] input = new CreateTag[] {
          new CreateTag("Name", "#FFFFFF"),
          new CreateTag("Test", "#FFFFFF"),
        };
        tagRepositoryMock.Setup(pre => pre.GetAll())
          .Returns(Enumerable.Empty<Tag>().AsQueryable());
        authUserMock.Setup(pre => pre.CurrentUser)
          .Returns(CreateValidUser());

        tagRepositoryMock.Setup(pre => pre.SaveInRange(It.IsAny<IEnumerable<Tag>>()));

        // Act
        Result<TagInformation[]> result = useCase.CreateTag(input);

        //Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Length.ShouldBe(2);
        tagRepositoryMock.Verify(pre => pre.SaveInRange(It.IsAny<IEnumerable<Tag>>()), Times.Once());
    }

    [Fact]
    public void CreateTag_ShouldntCreate_WhenTagAlreadyExistsInDb()
    {
        // Arrange
        CreateTag[] input = new[] { new CreateTag("Existing", "#FFFFFF") };

        Tag existing = new Tag(new Markivio.Domain.ValueObject.TagValueObject("Existing", "#FFFFFF")) { Id = Guid.NewGuid() };
        tagRepositoryMock.Setup(pre => pre.GetAll()).Returns(new[] { existing }.AsQueryable());
        authUserMock.Setup(pre => pre.CurrentUser).Returns(CreateValidUser());

        // Act
        Result<TagInformation[]> result = useCase.CreateTag(input);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ShouldBeOfType<AlreadyExistError>();
        tagRepositoryMock.Verify(pre => pre.SaveInRange(It.IsAny<IEnumerable<Tag>>()), Times.Never());
    }
}
