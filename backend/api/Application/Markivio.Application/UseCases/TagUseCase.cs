using System;
using FluentResults;
using Markivio.Domain.Entities;
using Markivio.Domain.Repositories;

namespace Markivio.Application.UseCases;

public interface ITagUseCase
{
    bool TagsExist(Tag[] tags);
}

public class TagUseCase(ITagRepository tagRepository) : ITagUseCase
{
    public bool TagsExist(Tag[] tags)
    {
        bool result = true;
        EqualityComparer<Tag> comparer = EqualityComparer<Tag>.Create((t1, t2) =>
        {
            if (ReferenceEquals(t1, t2))
                return true;

            if (t1 is null || t2 is null)
                return false;

            return t1.Name == t2.Name;
        });

        List<Tag> dbTags = tagRepository.GetAll()
            .ToList();

        foreach (Tag tag in tags)
            result &= dbTags.Contains(tag, comparer);

        return result;
    }
}