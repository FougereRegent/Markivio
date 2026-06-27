using Markivio.Application.UseCases;
namespace Markivio.Presentation.GraphQl.Tag;

public static class GetTagsAndNumberArticlesAssociated
{

    extension(IObjectTypeDescriptor<Query> descriptor)
    {
        public IObjectTypeDescriptor<Query> MapGetTagsAndNumberArticlesAssociated()
        {
            descriptor
                .Field("tagsStats")
                .Resolve(context =>
                {
					var tagUseCase = context.Services.GetRequiredService<ITagUseCase>();
					return tagUseCase.GetAllTagsAndNumberAssociatedArticle();
				})
                .UseOffsetPaging(options: GraphqlOptions.OffsetPagingOptions)
                .UseSorting();
            return descriptor;
        }
    }
}
