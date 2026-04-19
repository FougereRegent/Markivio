using Markivio.Domain.Exceptions;
using Markivio.Domain.ValueObject;

namespace Markivio.Domain.Entities;

public sealed class Article : EntityWithTenancy
{
    private const string REGEX_SOURCE = @"^(?:http[s]?:\/\/.)?(?:www\.)?[-a-zA-ZÀ-ÿà-ÿ0-9@%._\+~#=]{2,256}\.[a-z]{2,6}\b(?:[-a-zA-ZÀ-ÿà-ÿ0-9@:%_\+.~#?&\/\/=]*)";
    private const int MAX_TAGS = 20;

    public string Title { get; set; } = string.Empty;

    public Folder? Folder { get; set; } = null;
    public ArticleContent ArticleContent { get; set; } = null!;

    public bool IsFramable { get; set; } = true;

    private Article() { }

    public Article(ArticleContent articleContent, string title, bool isFramable)
    {
        ArticleContent = articleContent ?? throw new ArgumentNullException(nameof(articleContent));

        if (string.IsNullOrEmpty(title))
            throw new EmptyException("title cannot be empty", "EMPTY_ARTICLETITLE");

        Title = title;
        IsFramable = isFramable;
    }

	public void Update(string title, string source, string description, bool isFramable, IReadOnlyList<TagValueObject> tags) {

        if (string.IsNullOrEmpty(title))
            throw new EmptyException("title cannot be empty", "EMPTY_ARTICLETITLE");

		ArticleContent.Update(source: source,
				description: description,
				tags: tags);

		Title = title;
		IsFramable = isFramable;
	}
}
