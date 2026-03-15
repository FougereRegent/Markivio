using Markivio.Presentation.Interceptor;
using Markivio.Presentation.GraphQl;
using Markivio.Presentation.Middleware;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;

namespace Markivio.Presentation.Config;

public static class GraphQlConfig
{
    public static WebApplicationBuilder ConfigGraphQl(this WebApplicationBuilder builder)
    {
        builder.Services.AddGraphQLServer()
        .AddAuthorization()
        .AddHttpRequestInterceptor<AuthUserInterceptor>()
        .UseField<AuthMiddleware>()
        .AddQueryType<QueryType>()
        .AddMutationType<MutationType>()
		.AddInstrumentation()
#if DEBUG
        .ModifyRequestOptions(options =>
        {
            options.IncludeExceptionDetails = true;
            options.ExecutionTimeout = TimeSpan.FromMinutes(2);
        });
#endif

		builder.ConfigOpenTelemetry();
        return builder;
    }

	private static WebApplicationBuilder ConfigOpenTelemetry(this WebApplicationBuilder builder) {
		builder.Services.AddOpenTelemetry()
			.WithTracing(tracing => {
					tracing.AddAspNetCoreInstrumentation();
					tracing.AddHttpClientInstrumentation();
					tracing.AddHotChocolateInstrumentation();
					tracing.AddEntityFrameworkCoreInstrumentation();
					tracing.AddOtlpExporter();
					})
			.WithMetrics(metrics => {
					metrics.AddHttpClientInstrumentation();
					metrics.AddAspNetCoreInstrumentation();
					metrics.AddOtlpExporter();
					});
		return builder;
	}
