using Markivio.Presentation.GraphQl;
using Markivio.Presentation.Interceptor;

namespace Markivio.Presentation.Config;

public static class GraphQlConfiguration
{
    public static void GraphQlConfig(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddGraphQLServer()
          .AddAuthorization()
          .AddHttpRequestInterceptor<AuthUserInterceptor>()
          .AddQueryType<QueryType>();
    }
}
