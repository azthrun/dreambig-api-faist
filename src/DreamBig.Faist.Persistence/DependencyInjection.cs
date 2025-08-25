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
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            ArgumentException.ThrowIfNullOrEmpty(connectionString, "Connection string 'DefaultConnection' not found.");

            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            ArgumentException.ThrowIfNullOrEmpty(dbUser, "Database user 'DB_USER' not found.");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            ArgumentException.ThrowIfNullOrEmpty(dbPassword, "Database password 'DB_PASSWORD' not found.");

            connectionString = connectionString.Replace("#{DB_USER}#", dbUser).Replace("#{DB_PASSWORD}#", dbPassword);
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
