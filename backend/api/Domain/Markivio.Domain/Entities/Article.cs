using Markivio.Domain.Exceptions;

namespace Markivio.Domain.Entities;

public sealed class Article : EntityWithTenancy
{
    private const string REGEX_SOURCE = @"^(?:http[s]?:\/\/.)?(?:www\.)?[-a-zA-ZÀ-ÿà-ÿ0-9@%._\+~#=]{2,256}\.[a-z]{2,6}\b(?:[-a-zA-ZÀ-ÿà-ÿ0-9@:%_\+.~#?&\/\/=]*)";
    private const int TAGS_LIMIT = 20;

    public string Title { get; set; } = string.Empty;

    public Folder? Folder { get; set; } = null;
    public ArticleContent ArticleContent { get; set; } = null!;
    public List<Tag> Tags { get; set; } = new List<Tag>();

    public bool IsFramable { get; set; } = true;

    private Article() { }

    public Article(ArticleContent articleContent, string title, bool isFramable, List<Tag> tags)
    {
        ArticleContent = articleContent ?? throw new ArgumentNullException(nameof(articleContent));

        if (string.IsNullOrEmpty(title))
            throw new EmptyException("title cannot be empty", "EMPTY_ARTICLETITLE");

        if (tags is { Count: > TAGS_LIMIT })
            throw new TagLimitExceededException($"you cannot add more {TAGS_LIMIT} tags");

        Title = title;
        IsFramable = isFramable;
        Tags = tags;
    }

    public void Update(string title, string? description, IReadOnlyList<Tag> tags)
    {

        if (string.IsNullOrEmpty(title))
            throw new EmptyException("title cannot be empty", "EMPTY_ARTICLETITLE");

        if (tags is { Count: > TAGS_LIMIT })
            throw new TagLimitExceededException($"you cannot add more {TAGS_LIMIT} tags");

        ArticleContent.Update(description: description);

        Title = title;
        Tags.Clear();
        Tags.AddRange(tags);
    }

    public void AddTags(IReadOnlyList<Tag> tags)
    {
        if (tags.Count + Tags.Count > TAGS_LIMIT)
            throw new TagLimitExceededException($"you cannot add more {TAGS_LIMIT} tags");

        Tags.AddRange(tags);
    }

    public void RemoveTags(IReadOnlyList<Tag> tags)
    {
        foreach (Tag tag in tags)
        {
            Tag? removeTag = Tags.FirstOrDefault(pre => pre.Id == tag.Id);
            if (removeTag != null)
                Tags.Remove(removeTag);
        }
    }
}
