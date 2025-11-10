using FluentResults;
using Markivio.Domain.Errors;
using Markivio.Extensions;

namespace Markivio.Domain.Entities;

public sealed class Article : Entity, IModelValidation
{
    private const string REGEX_SOURCE = @"^(?:http[s]?:\/\/.)?(?:www\.)?[-a-zA-ZÀ-ÿà-ÿ0-9@%._\+~#=]{2,256}\.[a-z]{2,6}\b(?:[-a-zA-ZÀ-ÿà-ÿ0-9@:%_\+.~#?&\/\/=]*)";

    public string Title { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public Folder? Folder { get; set; } = null;
    public List<Tag> Tags { get; set; } = new List<Tag>();

    public Result Validate()
    {
        Result resultUser = Result.FailIf(User is null, new NullFieldError(nameof(User)));
        Result resultTitle = Result.FailIf(string.IsNullOrEmpty(Title), new ShouldNotBeEmptyError(nameof(Title)));
        Result resultSource = Result.FailIf(RegexExt.IsNotMatch(Source, REGEX_SOURCE), new FormatUnexpectedError(nameof(Source)));

        return Result.Merge(resultUser, resultTitle, resultSource);
    }
}
