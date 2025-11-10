using FluentResults;
using Markivio.Domain.Errors;
using Markivio.Extensions;

namespace Markivio.Domain.Entities;


public sealed class User : Entity, IModelValidation
{
    private const string REGEX_EMAIL = @"^[\w\-\.]+@([\w-]+\.)+[\w-]{2,}$";
    private const string REGEX_FIRSTNAME_AND_LASTNAME = @"^[A-Za-zÀ-ÿà-ÿ\-\'’]+(?:\s[\.\'’\,A-Za-zÀ-ÿà-ÿ\-]+)*$";

    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AuthId { get; set; } = string.Empty;

    public Result Validate()
    {
        Result emailResult = Result.FailIf(RegexExt.IsNotMatch(Email, REGEX_EMAIL),
            new FormatUnexpectedError(propertyName: nameof(Email)));
        Result firstNameResult = Result.FailIf(RegexExt.IsNotMatch(FirstName, REGEX_FIRSTNAME_AND_LASTNAME),
            new FormatUnexpectedError(propertyName: nameof(FirstName)));
        Result lastNameResult = Result.FailIf(RegexExt.IsNotMatch(LastName, REGEX_FIRSTNAME_AND_LASTNAME),
            new FormatUnexpectedError(propertyName: nameof(LastName)));

        return Result.Merge(emailResult, firstNameResult, lastNameResult);
    }
}
