using DreamBig.Faist.Application.Abstractions;
using DreamBig.Faist.Domain.Entities;

namespace DreamBig.Faist.Infrastructure.Services;

public sealed class WeatherForecastService : IWeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public IReadOnlyList<WeatherForecast> GetForecast(int days = 5)
    {
        var forecast = Enumerable.Range(1, days).Select(index =>
            new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            )).ToArray();
        return forecast;
    }
}
