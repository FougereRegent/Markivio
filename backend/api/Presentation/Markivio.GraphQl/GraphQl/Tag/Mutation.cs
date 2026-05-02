using Markivio.Application.Dto;
using Markivio.Application.UseCases;

namespace Markivio.Presentation.GraphQl;

public partial class Mutation
{
    public async Task<TagInformation[]> CreateTags(ITagUseCase tagUseCase,
        List<CreateTag> createTags,
        CancellationToken cancellationToken = default)
    {
        FluentResults.Result<TagInformation[]> resultCreate = tagUseCase.CreateTag(createTags.ToArray());
        return resultCreate.ThrowIfResultIsFailed();
    }
}
