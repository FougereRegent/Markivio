using Markivio.Persistence.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Markivio.DbUpdater;

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
            .ConfigureServices(service =>
            {
                service.AddDbContext<MarkivioContext>(options =>
                {
                    options.UseNpgsql("", b => b.MigrationsAssembly("Markivio.DbUpdater"));
                });
            });
}
