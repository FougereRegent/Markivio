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
builder.Services.AddHealthChecks();
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

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseRouting();
app.MapGraphQL();

app.UseHealthChecks("/health-check");
app.MapFallbackToFile("index.html");

if(ShouldRunMigration(app, args)){
	await ApplyMigration(app);
	if(ShouldExitAfterMigration(args)) return;
}

app.Run();


static bool ShouldRunMigration(WebApplication application, string[] arguments) {
	return application.Environment.IsDevelopment() || arguments
		.Any(pre => pre.Contains("migrate", StringComparison.CurrentCultureIgnoreCase));
}

static bool ShouldExitAfterMigration(string[] arguments) {
	return arguments
		.Any(pre => pre.Contains("migrate", StringComparison.CurrentCultureIgnoreCase));
}

static async Task ApplyMigration(WebApplication application) {
	using var scope = application.Services.CreateScope();
	var db = scope.ServiceProvider.GetRequiredService<Markivio.Persistence.Config.MarkivioContext>();
	await db.Database.MigrateAsync();
}
