using DreamBig.Faist.Domain.Entities;

namespace DreamBig.Faist.Application.Abstractions;

public interface IWeatherForecastService
{
    IReadOnlyList<WeatherForecast> GetForecast(int days = 5);
}
