using FluentResults;

namespace Markivio.Domain.Entities;

public sealed class Folder : EntityWithTenancy, IModelValidation
{
    public string Name { get; set; } = string.Empty;
    public List<Article> Articles { get; set; } = new List<Article>();

    public Result Validate()
    {
        throw new NotImplementedException();
    }
}
