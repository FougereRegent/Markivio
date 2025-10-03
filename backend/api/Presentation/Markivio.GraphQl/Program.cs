using Scalar.AspNetCore;
using Markivio.Presentation.Dto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

Action<ScalarOptions> scalarOptions = options =>
{
    options.WithTitle("Markivio API");
};

Console.WriteLine(app.Environment.IsDevelopment());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapScalarApiReference("/api-docs", scalarOptions);
    app.MapScalarApiReference("/docs", scalarOptions);
}

app.MapGet("/", () =>
{
    return new DefaultMessageDto("test");
})
.WithDisplayName("Default Route")
.WithDescription("The default route");

app.MapGet("/version", () =>
{
    return new VersionDto("markivio", "v0.0.0");
})
.WithDisplayName("Version")
.WithDescription("Get api version");


app.MapGet("/health-check", () =>
{
    return Task.FromResult(Results.Ok(new HealtkCheckDto(EnumHealthStatus.Alive)));
})
.WithDisplayName("Health Check")
.WithDescription("Get api health-check");

app.UseHttpsRedirection();
app.Run();
