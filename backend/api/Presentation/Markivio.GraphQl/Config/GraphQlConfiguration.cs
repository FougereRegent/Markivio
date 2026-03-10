using Markivio.Persistence.Config;
using Markivio.Presentation.GraphQl;
using Markivio.Presentation.Interceptor;
using Markivio.Presentation.Midleware;

namespace Markivio.Presentation.Config;

public static class GraphQlConfiguration
{
    public static void GraphQlConfig(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddGraphQLServer()
        .RegisterDbContextFactory<MarkivioContext>()
          .ModifyOptions(options =>
          {
              options.DefaultQueryDependencyInjectionScope = DependencyInjectionScope.Resolver;
              options.DefaultMutationDependencyInjectionScope = DependencyInjectionScope.Request;
          })
          .AddAuthorization()
          .AddHttpRequestInterceptor<AuthUserInterceptor>()
        .AddQueryType<QueryType>()
        .AddMutationType<MutationType>()
        .ModifyRequestOptions(o =>
        {
            o.IncludeExceptionDetails = true;
        });
    }
}
