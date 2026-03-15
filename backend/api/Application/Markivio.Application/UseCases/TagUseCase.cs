using FluentResults;
using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Application.Dto;
using Markivio.Application.Mapper;
using Markivio.Domain.Auth;
using Markivio.Application.Errors;
using Markivio.Domain.Exceptions;

namespace Markivio.Application.UseCases;

public enum TagExistConditionEnum
{
    Id,
    Name,
}

public interface ITagUseCase
{
	bool TagsExist<T>(IEnumerable<T> values, TagExistConditionEnum conditionEnum);
    Result<TagInformation[]> CreateTag(CreateTag[] creatingTags);
    IQueryable<TagInformation> GetAllTags(string tagName);
}

public class TagUseCase(ITagRepository tagRepository, IAuthUser authUser) : ITagUseCase
{
    public IQueryable<TagInformation> GetAllTags(string tagName)
    {
        IQueryable<Tag> query = tagRepository.GetAll();

        if (!(string.IsNullOrEmpty(tagName) && string.IsNullOrWhiteSpace(tagName)))
        {
            query = query.Where(pre => pre.TagValue.Name.StartsWith(tagName));
        }

        return query.ProjectionToTagInformation();
    }

    public Result<TagInformation[]> CreateTag(CreateTag[] creatingTags)
    {
        TagMapper mapper = new TagMapper();
        EqualityComparer<Tag> comparer = EqualityComparer<Tag>.Create((tag1, tag2) =>
        {
            if (tag1 is null || tag2 is null)
                return false;

            return tag1.TagValue.Name == tag2.TagValue.Name;
        }, tag => tag.TagValue.Name.GetHashCode());

        Tag[] tags = Array.Empty<Tag>();
        try
        {
            tags = creatingTags.Select(mapper.Map)
              .Select(pre => { pre.User = authUser.CurrentUser; return pre; })
              .ToArray();
        }
        catch (DomainException ex)
        {
            return Result.Fail(DomainError.Create(ex));
        }

        HashSet<Tag> hash = new HashSet<Tag>(tags, comparer);

        if (hash.Count != tags.Length)
            return Result.Fail(new DuplicatedItemsError());

        if (TagsExistByName(tags.Select(pre => pre.TagValue.Name).ToList()))
            return Result.Fail(new AlreadyExistError("Tag already exist"));

        tagRepository.SaveInRange(hash);

        return Result.Ok(hash.Select(pre => mapper.MapToTagInformation(pre))
                .ToArray());
    }

	public bool TagsExist<T>(IEnumerable<T> values, TagExistConditionEnum conditionEnum) =>
		conditionEnum switch {
			TagExistConditionEnum.Id when typeof(T) == typeof(Guid) => TagExistByIds(values.Cast<Guid>()),
			TagExistConditionEnum.Name when typeof(T) == typeof(string) => TagsExistByName(values.Cast<string>()),
			_ => throw new ArgumentException()
		};

    private bool TagsExistByName(IEnumerable<string> tagNames)
    {
        int nbTags = tagRepository.GetAll()
           .Count(pre => tagNames.Contains(pre.TagValue.Name));

        return nbTags == tagNames.Count();
    }

    private bool TagExistByIds(IEnumerable<Guid> tagIds)
    {
        int nbTags = tagRepository.GetAll()
           .Count(pre => tagIds.Contains(pre.Id));

        return nbTags == tagIds.Count();
    }
}
