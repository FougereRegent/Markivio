using Markivio.Presentation.GraphQl;

namespace Markivio.Presentation.Config;

public static class GraphQlConfiguration
{
    public static void GraphQlConfig(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddGraphQLServer()
          .AddQueryType<Query>();
    }
}
