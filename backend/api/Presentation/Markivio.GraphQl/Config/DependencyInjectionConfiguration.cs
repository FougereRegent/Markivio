using Markivio.Application.UseCases;
using Markivio.Domain.Auth;
using Markivio.Domain.Repositories;
using Markivio.Persistence;
using Markivio.Persistence.Config;
using Markivio.Persistence.Repositories;
using Markivio.Presentation.Dto;
using Microsoft.EntityFrameworkCore;
using Markivio.Auth;

namespace Markivio.Presentation.Config;

public static class DependencyInjectionConfiguration
{
    public static void ConfigureDependencyInjection(this IServiceCollection servicesCollection, EnvConfig config)
    {
        servicesCollection.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
        servicesCollection.AddScoped<IAuthUser, AuthUser>();
        servicesCollection.AddDbContext<MarkivioContext>(options =>
        {
            options.UseNpgsql(config.ConnectionString);
            //options.LogTo(Console.WriteLine, LogLevel.Debug)
            //.EnableSensitiveDataLogging();
        });
        servicesCollection.AddHttpClient();
        servicesCollection.AddMemoryCache();

        servicesCollection.ConfigureRepositories();
        servicesCollection.ConfigureUseCases();

    }

    private static void ConfigureUseCases(this IServiceCollection servicesCollection)
    {
        servicesCollection.AddScoped<IUserUseCase, UserUseCase>();
        servicesCollection.AddScoped<IArticleUseCase, ArticleUseCase>();
    }

    private static void ConfigureRepositories(this IServiceCollection servicesCollection)
    {
        servicesCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        servicesCollection.AddScoped<IArticleRepository, ArticleRepository>();
        servicesCollection.AddScoped<IUserRepository, UserRepository>();
        servicesCollection.AddScoped<ITagRepository, TagRepository>();
        servicesCollection.AddScoped<IFolderRepository, FolderRepository>();
    }
}

