using FluentResults;
using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;
using Markivio.Application.Dto;
using Markivio.Application.Mapper;
using Markivio.Domain.Auth;
using Markivio.Application.Errors;

namespace Markivio.Application.UseCases;

public interface ITagUseCase
{
    bool TagsExist(Tag[] tags);
    Result<TagInformation[]> CreateTag(CreateTag[] creatingTags);
	ValueTask<Result<TagInformation[]>> SearchTagsByName(string tagName, CancellationToken token = default);
    IQueryable<TagInformation> GetAll();
}

public class TagUseCase(ITagRepository tagRepository, IAuthUser authUser) : ITagUseCase
{
    public IQueryable<TagInformation> GetAll() =>
      tagRepository.GetAll()
          .ProjectionToTagInformation();

    public Result<TagInformation[]> CreateTag(CreateTag[] creatingTags)
    {
        TagMapper mapper = new TagMapper();
        EqualityComparer<Tag> comparer = EqualityComparer<Tag>.Create((tag1, tag2) =>
        {
            if (tag1 is null || tag2 is null)
                return false;

            return tag1.Name == tag2.Name;
        }, tag => tag.Name.GetHashCode());

        Tag[] tags = creatingTags.Select(mapper.CreateTagToTag)
          .Select(pre => { pre.User = authUser.CurrentUser; return pre; })
          .ToArray();

        HashSet<Tag> hash = new HashSet<Tag>(tags, comparer);

        if (hash.Count != tags.Length)
            return Result.Fail(new DuplicatedItemsError());

        if (TagsExist(tags))
            return Result.Fail(new AlreadyExistError("Tag already exist"));

        Result result = hash.Select(pre => pre.Validate()).Merge();
        if (result.IsFailed)
            return result;

        tagRepository.SaveInRange(hash);

        return Result.Ok(hash.Select(pre => mapper.TagToTagInformation(pre)).ToArray());
    }

    public bool TagsExist(Tag[] tags)
    {
        bool result = false;
        EqualityComparer<Tag> comparer = EqualityComparer<Tag>.Create((t1, t2) =>
        {
            if (ReferenceEquals(t1, t2))
                return true;

            if (t1 is null || t2 is null)
                return false;

            return t1.Id == t2.Id || t1.Name == t2.Name;
        }, tag => tag.GetHashCode());

        List<Tag> dbTags = tagRepository.GetAll()
            .ToList();

        foreach (Tag tag in tags)
            result |= dbTags.Contains(tag, comparer);

        return result;
    }

    public async ValueTask<Result<TagInformation[]>> SearchTagsByName(string tagName, CancellationToken token = default)
    {
		if(string.IsNullOrEmpty(tagName))
			return Result.Fail("");

		TagMapper mapper = new TagMapper();
		List<Tag> tags = await tagRepository.SearchTagByName(tagName);
	
		return Result.Ok(tags.Select(pre => mapper.TagToTagInformation(pre)).ToArray());
    }
}
