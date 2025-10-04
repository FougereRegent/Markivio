using Scalar.AspNetCore;
using Markivio.Presentation.Endpoints;

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

EndpointStatus.ConfigRoute(app);

app.UseHttpsRedirection();
app.Run();
