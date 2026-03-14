using Markivio.Presentation.Interceptor;
using Markivio.Presentation.GraphQl;
using Markivio.Presentation.Middleware;

namespace Markivio.Presentation.Config;

public static class GraphQlConfig {
			public static WebApplicationBuilder ConfigGraphQl(this WebApplicationBuilder builder) {
			builder.Services.AddGraphQLServer()
			.AddAuthorization()
			.AddHttpRequestInterceptor<AuthUserInterceptor>()
			.UseField<AuthMiddleware>()
			.AddQueryType<QueryType>()
			.AddMutationType<MutationType>()
#if DEBUG
			.ModifyRequestOptions(options => {
					options.IncludeExceptionDetails = true;
					options.ExecutionTimeout = TimeSpan.FromMinutes(2);
					});
#endif

		return builder;
	}
}
