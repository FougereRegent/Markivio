using Markivio.Auth;
using Markivio.Domain.Auth;
using Markivio.Domain.Repositories;
using Markivio.Persistence.Config;
using Markivio.Persistence;
using Markivio.Persistence.Repositories;
using Markivio.Application.UseCases;
using Markivio.Presentation.Dto;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Markivio.Presentation.Config;

public static class ConfigServiceInjection {
	public static WebApplicationBuilder ConfigDI(this WebApplicationBuilder builder, EnvConfig config) {
		builder.ConfigInfraServices(config.CorsOrigin)
			.ConfigAuth()
			.ConfigDB(config)
			.ConfigRepository()
			.ConfigServices();
		return builder;
	}

	private static WebApplicationBuilder ConfigDB(this WebApplicationBuilder builder, EnvConfig config) {
		builder.Services.AddDbContext<MarkivioContext>(options => {
            options.UseNpgsql(config.ConnectionString)
                          .UseCamelCaseNamingConvention();
				});
		return builder;
	}

	private static WebApplicationBuilder ConfigRepository(this WebApplicationBuilder builder) {
		builder.Services.AddScoped<IUnitOfWork, UnitOfWork>()
			.AddScoped<IUserRepository, UserRepository>()
			.AddScoped<ITagRepository, TagRepository>()
			.AddScoped<IArticleRepository, ArticleRepository>();
		return builder;
	}

	private static WebApplicationBuilder ConfigServices(this WebApplicationBuilder builder) {
		builder.Services.AddScoped<IUserUseCase, UserUseCase>()
			.AddScoped<IArticleUseCase, ArticleUseCase>()
			.AddScoped<ITagUseCase, TagUseCase>();
		return builder;
	}

	private static WebApplicationBuilder ConfigAuth(this WebApplicationBuilder builder) {
		builder.Services.AddScoped<IAuthUser, AuthUser>();
		return builder;
	}

	private static WebApplicationBuilder ConfigInfraServices(this WebApplicationBuilder builder, string corsOrigins) {
		// In dev we keep a permissive policy; outside dev we expect explicit origins.
		string[] origins = corsOrigins.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

		builder.Services.AddCors(options => {
				options.AddPolicy("AllowAllOrigins", policyBuilder => {
						if (builder.Environment.IsDevelopment()) {
							policyBuilder.AllowAnyOrigin();
						} else if (origins.Length > 0) {
							policyBuilder.WithOrigins(origins);
						}
						policyBuilder.AllowAnyHeader()
							.AllowAnyMethod();
						});
				})
		.AddMemoryCache()
		.AddHttpClient()
		.AddSerilog(options => {
				options.WriteTo.Async(pre => {
						pre.Console();
						pre.OpenTelemetry();
						});
				});
		return builder;
	}
}
