using Markivio.Presentation.Middleware;
using Markivio.Presentation.GraphQl.User;
using Markivio.Presentation.GraphQl.Article;
using Markivio.Presentation.GraphQl.Tag;

namespace Markivio.Presentation.GraphQl;

public partial class Mutation;

public class MutationType : ObjectType<Mutation>
{
    protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor.Authorize();

        descriptor.MapUserMutation()
        .MapTagMutation()
        .MapCreateArticle();
    }
}
