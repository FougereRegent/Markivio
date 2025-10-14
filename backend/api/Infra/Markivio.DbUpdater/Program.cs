using Markivio.Persistence.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Markivio.Extensions.HostingExtensions;

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
                service.AddDbContext<MarkivioContext>(options =>
                {
                    options.UseNpgsql(config?.ConnectionString ?? string.Empty, b => b.MigrationsAssembly("Markivio.DbUpdater"));
                });
            });
}
