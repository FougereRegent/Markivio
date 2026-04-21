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
	public string? Description { get; set; } = null;

    private ArticleContent() { }

    public ArticleContent(string source, string content, string? description)
    {
        if (string.IsNullOrEmpty(source))
            throw new EmptyException("source cannot be empty", "EMPTY_ARTICLESOURCE");

        if (!Regex.IsMatch(source, REGEX_SOURCE))
            throw new PatternException($"{source} didn't fit with url format", "FORMAT_ARTICLE_SOURCE");

        Source = source;
        Content = content;
        Description = description;
    }

    public void Update(string? description)

    {
        Description = description;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Source;
        yield return Description!;
        yield return Content;
    }
}
