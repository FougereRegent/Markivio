using Scalar.AspNetCore;
using Markivio.Presentation.Dto;
using Markivio.Presentation.Config;
using Microsoft.EntityFrameworkCore;


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
builder.Services.AddAuth0(config, !builder.Environment.IsDevelopment());
builder.ConfigDI(config);
builder.ConfigGraphQl();


var app = builder.Build();
app.UseAuth();

Action<ScalarOptions> scalarOptions = options =>
{
    options.WithTitle("Markivio API");
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapScalarApiReference("/api-docs", scalarOptions);
    app.MapScalarApiReference("/docs", scalarOptions);
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseRouting();
app.MapGraphQL();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Markivio.Persistence.Config.MarkivioContext>();
    bool runMigrations =
        app.Environment.IsDevelopment()
        || builder.Configuration.GetValue<bool>("MARKIVIO_RUN_MIGRATIONS");

    if (runMigrations)
        await db.Database.MigrateAsync();
}
app.Run();
