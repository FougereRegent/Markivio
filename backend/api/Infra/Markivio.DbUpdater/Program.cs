using System;
using Markivio.Persistence.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Markivio.DbUpdater;

public static class Program
{
    public static void Main()
    {

    }

    //public static IHostBuilder CreateHostBuilder(string[] args)
    //{
    //    HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
    //    builder.Services.AddDbContext<MarkivioContext>(options =>
    //    {
    //    });
    //    return builder;
    //}
}
