using Markivio.Presentation.Dto;
using Scalar.AspNetCore;

namespace Markivio.Presentation.Config;

public static class ApiConfig {

	private static readonly Action<ScalarOptions> scalarOptions = options =>
	{
		options.WithTitle("Markivio API");
	};

	extension(WebApplicationBuilder builder) {
		public WebApplicationBuilder ConfigJson() {
			builder.Services.ConfigureHttpJsonOptions(opts => {
					opts.SerializerOptions.PropertyNameCaseInsensitive = true;
					opts.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.KebabCaseLower;
					});
			return builder;
		}

		public WebApplicationBuilder ConfigHealthCheck() {
			builder.Services.AddHealthChecks();
			return builder;
		}

		public WebApplicationBuilder ConfigAuth(EnvConfig config) {
			builder.Services.AddAuth0(config, !builder.Environment.IsDevelopment());
			return builder;
		}
	}

	extension(WebApplication app) {
		public WebApplication ConfigAuth() {
			return app;
		}

		public WebApplication ConfigScalar() {
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.MapScalarApiReference();
				app.MapScalarApiReference("/api-docs", scalarOptions);
				app.MapScalarApiReference("/docs", scalarOptions);
			}
			return app;
		}

		public WebApplication ConfigApi() {
			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseCors("AllowAllOrigins");
			app.UseHttpsRedirection();
			app.UseRouting();
			app.MapGraphQL();

			app.UseHealthChecks("/health-check");
			app.MapFallbackToFile("index.html");
			return app;
		}
	}
}
