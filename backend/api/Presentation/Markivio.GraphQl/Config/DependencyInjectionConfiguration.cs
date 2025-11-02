using Markivio.Application.Users;
using Markivio.Domain.Repositories;
using Markivio.Persistence;
using Markivio.Persistence.Config;
using Markivio.Persistence.Repositories;
using Markivio.Presentation.Dto;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Presentation.Config;

public static class DependencyInjectionConfiguration
{
    public static void ConfigureDependencyInjection(this IServiceCollection servicesCollection, EnvConfig config)
    {
        servicesCollection.AddDbContext<MarkivioContext>(options =>
        {
            options.UseNpgsql(config.ConnectionString);
        });
        servicesCollection.AddHttpClient();
        servicesCollection.AddMemoryCache();

        servicesCollection.ConfigureRepositories();
        servicesCollection.ConfigureUseCases();

    }

    private static void ConfigureUseCases(this IServiceCollection servicesCollection)
    {
        servicesCollection.AddScoped<IUserUseCase, UserUseCase>();
        servicesCollection.AddScoped<IAuthUser, UserUseCase>();
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

