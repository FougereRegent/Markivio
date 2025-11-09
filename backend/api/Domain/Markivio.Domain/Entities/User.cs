using FluentResults;
using Markivio.Domain.Errors;
using System.Text.RegularExpressions;

namespace Markivio.Domain.Entities;


public sealed class User : Entity, IModelValidation
{
    private const string REGEX_EMAIL = @"^[\w\-\.]+@([\w-]+\.)+[\w-]{2,}$";
    private const string REGEX_FIRSTNAME_AND_LASTNAME = @"^[A-ZÀ-Ÿ][a-zà-ÿ'-]+(?: [A-ZÀ-Ÿ][a-zà-ÿ'-]+)*$";

    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AuthId { get; set; } = string.Empty;

    public Result Validate()
    {
        Result emailResult = Result.FailIf(!Regex.IsMatch(Email, REGEX_EMAIL),
            new FormatUnexpectedError(propertyName: nameof(Email)));
        Result firstNameResult = Result.FailIf(!Regex.IsMatch(FirstName, REGEX_FIRSTNAME_AND_LASTNAME),
            new FormatUnexpectedError(propertyName: nameof(FirstName)));
        Result lastNameResult = Result.FailIf(!Regex.IsMatch(LastName, REGEX_FIRSTNAME_AND_LASTNAME),
            new FormatUnexpectedError(propertyName: nameof(LastName)));

        return Result.Merge(emailResult, firstNameResult, lastNameResult);
    }
}
