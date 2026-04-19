using System.Text.RegularExpressions;
using Markivio.Domain.Exceptions;
using Markivio.Domain.ValueObject;

namespace Markivio.Domain.Entities;

public sealed class ArticleContent : BaseValueObject
{
    private const int TAGS_LIMIT = 20;
    private const string REGEX_SOURCE = @"^(?:http[s]?:\/\/.)?(?:www\.)?[-a-zA-ZÀ-ÿà-ÿ0-9@%._\+~#=]{2,256}\.[a-z]{2,6}\b(?:[-a-zA-ZÀ-ÿà-ÿ0-9@:%_\+.~#?&\/\/=]*)";

    public string Source { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<TagValueObject> Tags { get; set; } = new List<TagValueObject>();
    public string? Description { get; set; } = null;

    private ArticleContent() { }

    public ArticleContent(string source, string content, List<TagValueObject> tags, string? description)
    {
        if (string.IsNullOrEmpty(source))
            throw new EmptyException("source cannot be empty", "EMPTY_ARTICLESOURCE");

        if (!Regex.IsMatch(source, REGEX_SOURCE))
            throw new PatternException($"{source} didn't fit with url format", "FORMAT_ARTICLE_SOURCE");

        if (tags is { Count: > TAGS_LIMIT })
            throw new TagLimitExceededException($"you cannot add more {TAGS_LIMIT} tags");

        Source = source;
        Content = content;
        Tags = tags;
        Description = description;
    }

    public void AddTags(IReadOnlyList<TagValueObject> tags)
    {
        if (tags.Count + Tags.Count > 20)
            throw new TagLimitExceededException($"you cannot add more {TAGS_LIMIT} tags");

        Tags.AddRange(tags);
    }

    public void RemoveTags(IReadOnlyList<TagValueObject> tags)
    {
        foreach (TagValueObject tag in tags)
        {
            TagValueObject? removeTag = Tags.FirstOrDefault(pre => pre.Name == tag.Name);
            if (removeTag != null)
                Tags.Remove(removeTag);
        }
    }

	public void Update(string source, string description, IReadOnlyList<TagValueObject> tags) {
        if (string.IsNullOrEmpty(source))
            throw new EmptyException("source cannot be empty", "EMPTY_ARTICLESOURCE");

        if (!Regex.IsMatch(source, REGEX_SOURCE))
            throw new PatternException($"{source} didn't fit with url format", "FORMAT_ARTICLE_SOURCE");

		AddTags(tags);

		Source = source;
		Description = description;
	}

    protected override IEnumerable<object> GetAtomicValues()
    {
		yield return Source;
		yield return Description!;
		yield return Tags;
		yield return Content;
    }
}
