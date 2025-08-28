using DreamBig.Faist.Application.Common.Interfaces;
using DreamBig.Faist.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DreamBig.Faist.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<FaistDbContext>((provider, options) =>
        {
            IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            ArgumentException.ThrowIfNullOrEmpty(connectionString, "Connection string 'DefaultConnection' not found.");

            string? dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            ArgumentException.ThrowIfNullOrEmpty(dbHost, "Database host 'DB_HOST' not found.");
            string? dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            ArgumentException.ThrowIfNullOrEmpty(dbPort, "Database port 'DB_PORT' not found.");
            string? dbUser = Environment.GetEnvironmentVariable("DB_USER");
            ArgumentException.ThrowIfNullOrEmpty(dbUser, "Database user 'DB_USER' not found.");
            string? dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            ArgumentException.ThrowIfNullOrEmpty(dbPassword, "Database password 'DB_PASSWORD' not found.");

            connectionString = connectionString
                .Replace("#{DB_HOST}", dbHost)
                .Replace("#{DB_PORT}", dbPort)
                .Replace("#{DB_USER}", dbUser)
                .Replace("#{DB_PASSWORD}", dbPassword);
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
