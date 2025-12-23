
using Markivio.Domain.Entities;
using Shouldly;

namespace Markivio.UnitTests.Application;


public class TagExistTests : BaseTagTests
{
    public static IEnumerable<object[]> DataTestIfExist()
    {
        List<Tag> dbTags = Enumerable.Range(1, 10)
          .Select(pre => new Tag { Name = $"Test{pre}", Id = Guid.NewGuid() })
          .ToList();

        yield return new object[] { dbTags, dbTags };

        List<Tag> inputSrc = Enumerable.Range(1, 10)
          .Select(pre => new Tag { Name = $"Src{pre}", Id = Guid.NewGuid() })
          .ToList();
        inputSrc.AddRange(dbTags.GetRange(2, 4));

        yield return new object[] { inputSrc, dbTags };
    }

    public static IEnumerable<object[]> DataTestIfNotExist()
    {
        List<Tag> dbTags = Enumerable.Range(1, 10)
          .Select(pre => new Tag { Name = $"Test{pre}", Id = Guid.NewGuid() })
          .ToList();

        List<Tag> inputSrc = Enumerable.Range(1, 10)
          .Select(pre => new Tag { Name = $"Src{pre}", Id = Guid.NewGuid() })
          .ToList();

        yield return new object[] { inputSrc, dbTags };

        inputSrc = Enumerable.Range(1, 100)
          .Select(pre => new Tag { Name = $"Src{pre}", Id = Guid.NewGuid() })
          .ToList();

        yield return new object[] { inputSrc, dbTags };
    }

    [Theory]
    [MemberData(nameof(DataTestIfExist))]
    public void TagExist_ShouldReturnTrue_WhenTagExist(List<Tag> source, List<Tag> dbSource)
    {
        //Arrange
        tagRepositoryMock.Setup(pre => pre.GetAll())
          .Returns(dbSource.AsQueryable());

        //Act
        bool result = useCase.TagsExist(source.ToArray());

        //Assert
        result.ShouldBeTrue();
    }

    [Theory]
    [MemberData(nameof(DataTestIfNotExist))]
    public void TagExist_ShouldReturnFalse_WhenTagDoesntExist(List<Tag> source, List<Tag> dbSource)
    {

        //Arrange
        tagRepositoryMock.Setup(pre => pre.GetAll())
          .Returns(dbSource.AsQueryable());

        //Act
        bool result = useCase.TagsExist(source.ToArray());

        //Assert
        result.ShouldBeFalse();
    }
}
