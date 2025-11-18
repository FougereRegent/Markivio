using Scalar.AspNetCore;
using Markivio.Presentation.Dto;
using Markivio.Presentation.Config;
using Markivio.Extensions.HostingExtensions;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.Config();
EnvConfig? config = builder.Configuration.BindEnvVariables<EnvConfig>();

if (config is null) return;
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();
builder.Services.AddAuth0(config);
builder.Services.ConfigureDependencyInjection(config);
builder.Services.GraphQlConfig();

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


//app.ConfigureStatusEndpoints();

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();
app.Run();
