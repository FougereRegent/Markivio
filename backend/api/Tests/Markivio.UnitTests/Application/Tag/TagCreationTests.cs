
using FluentResults;
using Markivio.Application.Dto;
using Markivio.Application.Errors;
using Markivio.Domain.Entities;
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
        authUserMock.Setup(pre => pre.CurrentUser)
          .Returns(new User());

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
        authUserMock.Setup(pre => pre.CurrentUser)
          .Returns(new User());

        // Act
        Result<TagInformation[]> result = useCase.CreateTag(input);

        //Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
    }

    [Fact]
    public void CreateTag_Should_Create()
    {
        //Arrange
        CreateTag[] input = new CreateTag[] {
          new CreateTag("Name", "#FFFFFF"),
          new CreateTag("Test", "#FFFFFF"),
        };
        authUserMock.Setup(pre => pre.CurrentUser)
          .Returns(new User());

        tagRepositoryMock.Setup(pre => pre.SaveInRange(It.IsAny<IEnumerable<Tag>>()));

        // Act
        Result<TagInformation[]> result = useCase.CreateTag(input);

        //Assert
        result.IsSuccess.ShouldBeTrue();
    }
}
