using Markivio.Persistence.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Markivio.Extensions.HostingExtensions;
using Markivio.Domain.Entities;
using Markivio.DbUpdater.ModelGenerator;
using Bogus;
using System.Linq;
using Markivio.Domain.Auth;

namespace Markivio.DbUpdater;

internal record EnvConfig(
    [EnvironmentVariable("MARKIVIO_CONNECTION_STRING")] string ConnectionString
    );

public class Program
{
    public static void Main(string[] args)
    {
        IHostBuilder hostBuilder = CreateHostBuilder((args));
        IHost host = hostBuilder.Build();

        SeedDataBase(host);

        host.Run();
    }

    // EF Core uses this method at design time to access the DbContext
    public static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, options) =>
            {
                options.AddEnvironmentVariables();
            })
            .ConfigureServices((context, service) =>
            {
                EnvConfig? config = context.Configuration.BindEnvVariables<EnvConfig>();
                service.AddScoped<IAuthUser>(ServiceProvider =>
                {
                    return null!;
                });
                service.AddDbContext<MarkivioContext>(options =>
                {
                    options.UseNpgsql(config?.ConnectionString ?? string.Empty, b => b.MigrationsAssembly("Markivio.DbUpdater"));
                });
            });

    private static void SeedDataBase(IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();
        MarkivioContext context = scope.ServiceProvider.GetRequiredService<MarkivioContext>();
        UserGenerator userGenerator = new UserGenerator();
        ArticleGenerator articleGenerator = new ArticleGenerator();
        TagGenerator tagGenerator = new TagGenerator();
        FolderGenerator folderGenerator = new FolderGenerator();

        List<User> users = userGenerator.Generate(100);
        List<Article> articles = articleGenerator.Generate(40_000);
        List<Tag> tags = tagGenerator.Generate(50);
        List<Folder> folders = folderGenerator.Generate(400);

        CreateUser(users, context);
        CreateArticles(articles, users, context);
        CreateFolder(folders, articles, users, context);
    }

    private static void CreateUser(List<User> users, MarkivioContext context)
    {
        context.User.AddRange(users);
        context.SaveChanges();
    }

    private static void CreateArticles(List<Article> articles, List<User> users, MarkivioContext context)
    {
        Faker faker = new Faker();

        foreach (Article article in articles)
        {
            article.User = faker.Random.ListItem(users);
            context.Add(article);
        }
        context.SaveChanges();
    }

    private static void CreateFolder(List<Folder> folders, List<Article> articles, List<User> users, MarkivioContext context)
    {
        Faker faker = new Faker();
        foreach (Folder folder in folders)
        {
            User user = faker.Random.ListItem(users);
            List<Article> selectedArticles = articles.Where(pre => pre.User == user).ToList();
            selectedArticles = faker.Random.ListItems(selectedArticles)
             .Distinct()
             .ToList();
            folder.Articles.AddRange(selectedArticles);
            folder.User = user;
        }

        context.Folder.AddRange(folders);
        context.SaveChanges();
    }
}
