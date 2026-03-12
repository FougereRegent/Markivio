using Markivio.Persistence.Config;
using Markivio.Presentation.GraphQl;
using Markivio.Presentation.Interceptor;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;

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
		.AddInstrumentation()
        .ModifyRequestOptions(o =>
        {
            o.IncludeExceptionDetails = true;
        });

		serviceCollection.AddOpenTelemetry()
			.WithTracing(tracing => {
					tracing.AddHttpClientInstrumentation();
					tracing.AddHotChocolateInstrumentation();
					tracing.AddAspNetCoreInstrumentation();
					tracing.AddEntityFrameworkCoreInstrumentation();
					tracing.AddOtlpExporter();
					})
			.WithMetrics(metrics => {
					metrics.AddAspNetCoreInstrumentation();
					metrics.AddHttpClientInstrumentation();
					metrics.AddOtlpExporter();
					})
			.WithLogging(logging => {
					logging.AddOtlpExporter();
					});
    }
}
