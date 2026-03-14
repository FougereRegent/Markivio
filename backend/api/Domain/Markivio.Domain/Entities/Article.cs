using Markivio.Domain.Exceptions;

namespace Markivio.Domain.Entities;

public sealed class Article : EntityWithTenancy
{
    private const string REGEX_SOURCE = @"^(?:http[s]?:\/\/.)?(?:www\.)?[-a-zA-ZÀ-ÿà-ÿ0-9@%._\+~#=]{2,256}\.[a-z]{2,6}\b(?:[-a-zA-ZÀ-ÿà-ÿ0-9@:%_\+.~#?&\/\/=]*)";
    private const int MAX_TAGS = 20;

    public string Title { get; set; } = string.Empty;

    public Folder? Folder { get; set; } = null;
    public ArticleContent ArticleContent { get; set; } = null!;

	public Article(ArticleContent articleContent, string title) {
		if(string.IsNullOrEmpty(title))
			throw new EmptyException("title cannot be empty", "EMPTY_ARTICLETITLE");
	}
}
