using Markivio.Application.Dto;
using Markivio.Application.UseCases;

namespace Markivio.Presentation.GraphQl;

public partial class Query {
    public IQueryable<TagInformation> Tags(ITagUseCase tagUseCase, string? tagName) =>
        tagUseCase.GetAllTags(tagName ?? string.Empty);
}
