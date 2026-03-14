using System.Text.RegularExpressions;
using Markivio.Domain.Exceptions;
using Markivio.Domain.ValueObject;

namespace Markivio.Domain.Entities;

public sealed class ArticleContent
{
    private const string REGEX_SOURCE = @"^(?:http[s]?:\/\/.)?(?:www\.)?[-a-zA-ZÀ-ÿà-ÿ0-9@%._\+~#=]{2,256}\.[a-z]{2,6}\b(?:[-a-zA-ZÀ-ÿà-ÿ0-9@:%_\+.~#?&\/\/=]*)";

    public string Source { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<TagValueObject> Tags { get; set; } = new List<TagValueObject>();
    public string? Description { get; set; } = null;

	private ArticleContent() {}

	public ArticleContent(string source, string content, List<TagValueObject> tags, string description) {
		const int TAGS_LIMIT = 20;
		if(!string.IsNullOrEmpty(source))
			throw new EmptyException("source cannot be empty", "EMPTY_ARTICLESOURCE");

		if(!Regex.IsMatch(source, REGEX_SOURCE))
			throw new PatternException($"{source} didn't fit with url format", "FORMAT_ARTICLE_SOURCE");

		if(tags is { Count:>TAGS_LIMIT})
			throw new TagLimitExceededException($"you cannot add more {TAGS_LIMIT} tags");
	}
}

public sealed class SoftTag
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}
