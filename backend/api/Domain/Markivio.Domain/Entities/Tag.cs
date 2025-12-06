using FluentResults;
using Markivio.Domain.Errors;
using Markivio.Extensions;

namespace Markivio.Domain.Entities;

public sealed class Tag : Entity, IModelValidation
{
    private const string REGEX_TAG_NAME = @"^[A-Za-zÀ-ÿà-ÿ\-\'’ 0-9 &#`\-_]{1,25}$";
    private const string REGEX_TAG_COLOR = @"^#[A-Fa-f0-9]{6}$";

    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public User User { get; set; } = null!;

    public Result Validate()
    {
        Result nameResult = Result.FailIf(RegexExt.IsNotMatch(Name, REGEX_TAG_NAME),
            new FormatUnexpectedError(propertyName: nameof(Name)));
        Result colorResult = Result.FailIf(RegexExt.IsNotMatch(Color, REGEX_TAG_COLOR),
            new FormatUnexpectedError(propertyName: nameof(Color)));

        return Result.Merge(nameResult, colorResult);
    }
}
