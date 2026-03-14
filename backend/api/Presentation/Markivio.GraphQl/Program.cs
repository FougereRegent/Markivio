using Scalar.AspNetCore;
using Markivio.Presentation.Dto;
using Markivio.Presentation.Config;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

EnvConfig config = new EnvConfig(
		Authority: builder.Configuration.GetValue<string>("MARKIVIO_AUTHORITY") ?? throw new ArgumentException(),
		Audience: builder.Configuration.GetValue<string>("MARKIVIO_AUDIENCE") ?? throw new ArgumentException(),
		ConnectionString: builder.Configuration.GetConnectionString("markivio") ?? throw new ArgumentException()
		);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();
builder.Services.AddAuth0(config);
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
app.UseEndpoints(endpoints => {
		endpoints.MapGraphQL();
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Markivio.Persistence.Config.MarkivioContext>();
	await db.Database.MigrateAsync();
}
app.Run();
