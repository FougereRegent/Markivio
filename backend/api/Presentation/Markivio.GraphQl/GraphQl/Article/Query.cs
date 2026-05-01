using Markivio.Application.Dto;
using Markivio.Application.UseCases;

namespace Markivio.Presentation.GraphQl;

public partial class Query {

    public IQueryable<ArticleInformation> Articles(IArticleUseCase articleUseCase, string? title, List<string>? tags)
    {
        return articleUseCase.FindByFilter(new ArticleFilters(
              title,
              tags
              ));
    }
}
