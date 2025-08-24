using DreamBig.Faist.Application;
using DreamBig.Faist.Application.Abstractions;
using DreamBig.Faist.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", (IWeatherForecastService service) => service.GetForecast())
    .WithName("GetWeatherForecast");

app.Run();

// WeatherForecast entity lives in Domain layer now.
