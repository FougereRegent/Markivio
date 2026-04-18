using Scalar.AspNetCore;
using Markivio.Presentation.Dto;
using Markivio.Presentation.Config;
using Microsoft.EntityFrameworkCore;
using Markivio.Presentation.Endpoint.Config;
using Markivio.Presentation.Endpoint.Version;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

string connectionString =
    builder.Configuration.GetConnectionString("markivio")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? builder.Configuration.GetValue<string>("MARKIVIO_CONNECTION_STRING")
    ?? throw new ArgumentException("Missing connection string. Provide ConnectionStrings__markivio or MARKIVIO_CONNECTION_STRING.");


EnvConfig config = new EnvConfig();
config.CONNECTION_STRING = connectionString;
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Configuration.Bind(config);
builder.Services.AddOpenApi();
builder
    .ConfigAuth(config)
    .ConfigHealthCheck()
    .ConfigDI(config)
    .ConfigGraphQl()
    .ConfigJson();

var app = builder.Build();
app.ConfigAuth()
    .ConfigScalar()
    .ConfigApi();

if (ShouldRunMigration(app, args))
{
    await ApplyMigration(app);
    if (ShouldExitAfterMigration(args)) return;
}

var api = app.MapGroup("/api");
api.GetConfig()
    .GetVersion();

app.Run();


static bool ShouldRunMigration(WebApplication application, string[] arguments)
{
    return application.Environment.IsDevelopment() || arguments
        .Any(pre => pre.Contains("migrate", StringComparison.CurrentCultureIgnoreCase));
}

static bool ShouldExitAfterMigration(string[] arguments)
{
    return arguments
        .Any(pre => pre.Contains("migrate", StringComparison.CurrentCultureIgnoreCase));
}

static async Task ApplyMigration(WebApplication application)
{
    using var scope = application.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<Markivio.Persistence.Config.MarkivioContext>();
    await db.Database.MigrateAsync();
}
