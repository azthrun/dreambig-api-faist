using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DreamBig.Faist.Persistence;

public class FaistDbContextFactory(IConfiguration configuration) : IDesignTimeDbContextFactory<FaistDbContext>
{
    private readonly IConfiguration _configuration = configuration;

    public FaistDbContext CreateDbContext(string[] args)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "Connection string 'DefaultConnection' not found.");
        }

        var dbUser = _configuration["DB_USER"];
        var dbPassword = _configuration["DB_PASSWORD"];

        connectionString = connectionString.Replace("#{DbUser}#", dbUser);
        connectionString = connectionString.Replace("#{DbPassword}#", dbPassword);

        var optionsBuilder = new DbContextOptionsBuilder<FaistDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new FaistDbContext(optionsBuilder.Options);
    }
}
