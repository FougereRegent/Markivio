using FluentResults;
using Markivio.Domain.Errors;
using Markivio.Extensions;

namespace Markivio.Domain.Entities;

public sealed class Article : Entity, IModelValidation
{
    private const string REGEX_SOURCE = @"^(?:http[s]?:\/\/.)?(?:www\.)?[-a-zA-ZÀ-ÿà-ÿ0-9@%._\+~#=]{2,256}\.[a-z]{2,6}\b(?:[-a-zA-ZÀ-ÿà-ÿ0-9@:%_\+.~#?&\/\/=]*)";
    private const int MAX_TAGS = 20;

    public string Title { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public Folder? Folder { get; set; } = null;
    public ArticleContent ArticleContent { get; set; } = null!;

    public Result Validate()
    {
        Result resultSource = Result.Ok();
        Result resultTags = Result.Ok();
        Result resultUser = Result.FailIf(User is null, new NullFieldError(nameof(User)));
        Result resultTitle = Result.FailIf(string.IsNullOrEmpty(Title), new ShouldNotBeEmptyError(nameof(Title)));

        if (ArticleContent != null)
        {
            resultSource = Result.FailIf(RegexExt.IsNotMatch(ArticleContent.Source, REGEX_SOURCE), new FormatUnexpectedError(nameof(ArticleContent.Source)));
            resultTags = Result.Merge(
                Result.FailIf(ArticleContent.Tags.Count > MAX_TAGS, new ExceedElementsError(MAX_TAGS, nameof(ArticleContent.Tags))));
        }

        return Result.Merge(resultUser, resultTitle, resultSource, resultTags);
    }
}
