using Markivio.Application.UseCases;
using Markivio.Domain.Entities;
using Markivio.Domain.ValueObject;
using Shouldly;

namespace Markivio.UnitTests.Application;

public sealed class TagExistTests : BaseTagTests
{
    [Fact]
    public void TagsExist_ById_ShouldReturnTrue_WhenAllIdsExist()
    {
        // Arrange
        Guid[] ids = Enumerable.Range(0, 5).Select(_ => Guid.NewGuid()).ToArray();
        Tag[] dbTags = ids.Select(id =>
            new Tag(new TagValueObject($"tag{id.ToString()[..6]}", "#FFFFFF")) { Id = id }).ToArray();

        tagRepositoryMock.Setup(pre => pre.GetAll())
          .Returns(dbTags.AsQueryable());

        // Act
        bool result = useCase.TagsExist<Guid>(ids, TagExistConditionEnum.Id);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void TagsExist_ById_ShouldReturnFalse_WhenSomeIdsAreMissing()
    {
        // Arrange
        Guid[] ids = Enumerable.Range(0, 5).Select(_ => Guid.NewGuid()).ToArray();
        Tag[] dbTags = ids.Take(3).Select(id =>
            new Tag(new TagValueObject($"tag{id.ToString()[..6]}", "#FFFFFF")) { Id = id }).ToArray();

        tagRepositoryMock.Setup(pre => pre.GetAll())
          .Returns(dbTags.AsQueryable());

        // Act
        bool result = useCase.TagsExist<Guid>(ids, TagExistConditionEnum.Id);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void TagsExist_ByName_ShouldReturnTrue_WhenAllNamesExist()
    {
        // Arrange
        string[] names = new[] { "Alpha", "Beta", "Gamma" };
        Tag[] dbTags = names.Select((name, i) =>
            new Tag(new TagValueObject(name, "#FFFFFF")) { Id = Guid.NewGuid() }).ToArray();

        tagRepositoryMock.Setup(pre => pre.GetAll())
          .Returns(dbTags.AsQueryable());

        // Act
        bool result = useCase.TagsExist<string>(names, TagExistConditionEnum.Name);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void TagsExist_ByName_ShouldReturnFalse_WhenSomeNamesAreMissing()
    {
        // Arrange
        string[] names = new[] { "Alpha", "Beta", "Gamma" };
        Tag[] dbTags =
        [
            new Tag(new TagValueObject("Alpha", "#FFFFFF")) { Id = Guid.NewGuid() },
            new Tag(new TagValueObject("Gamma", "#FFFFFF")) { Id = Guid.NewGuid() },
        ];

        tagRepositoryMock.Setup(pre => pre.GetAll())
          .Returns(dbTags.AsQueryable());

        // Act
        bool result = useCase.TagsExist<string>(names, TagExistConditionEnum.Name);

        // Assert
        result.ShouldBeFalse();
    }
}

