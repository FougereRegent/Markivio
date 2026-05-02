using Markivio.Presentation.GraphQl.Article;
using Markivio.Presentation.GraphQl.User;
using Markivio.Presentation.GraphQl.Tag;


namespace Markivio.Presentation.GraphQl;

public partial class Query;

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {

        descriptor
          .Authorize();

        descriptor.MapArticleQuery();
        descriptor.MapUserQuery();
        descriptor.MapTagQuery();
    }
}
