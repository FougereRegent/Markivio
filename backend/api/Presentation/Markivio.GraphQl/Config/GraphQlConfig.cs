using Markivio.Presentation.Interceptor;
using Markivio.Presentation.GraphQl;
using Markivio.Presentation.Middleware;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using Markivio.Presentation.ErrorFilters;

namespace Markivio.Presentation.Config;

public static class GraphQlConfig
{
    public static WebApplicationBuilder ConfigGraphQl(this WebApplicationBuilder builder)
    {
        builder.ConfigOpenTelemetry();
        builder.Services.AddGraphQLServer()
        .AddAuthorization()
        .AddHttpRequestInterceptor<AuthUserInterceptor>()
        .UseField<AuthMiddleware>()
        .AddErrorFilter<DomainErrorFilter>()
        .AddQueryType<QueryType>()
        .AddMutationType<MutationType>()
        .AddFiltering()
        .AddSorting()
        .AddInstrumentation()
#if DEBUG
        .ModifyRequestOptions(options =>
        {
            options.IncludeExceptionDetails = true;
            options.ExecutionTimeout = TimeSpan.FromMinutes(2);
        })
#endif
;
        return builder;
    }

    private static WebApplicationBuilder ConfigOpenTelemetry(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
                tracing.AddHotChocolateInstrumentation();
                tracing.AddEntityFrameworkCoreInstrumentation();
                tracing.AddOtlpExporter();
            })
            .WithMetrics(metrics =>
            {
                metrics.AddHttpClientInstrumentation();
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddOtlpExporter();
            })
            .WithLogging(logging =>
            {
                logging.AddOtlpExporter();
            });
        return builder;
    }
}
