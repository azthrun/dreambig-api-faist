using DreamBig.Faist.Api.Middleware;
using DreamBig.Faist.Application.Common.Extensions;
using DreamBig.Faist.Application.Tasks.Queries;
using DreamBig.Faist.Infrastructure;
using DreamBig.Faist.Persistence;
using FluentValidation;
using Mediator;

var builder = WebApplication.CreateSlimBuilder();
builder.WebHost.UseKestrelHttpsConfiguration();

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();
builder.Configuration.AddConfiguration(configuration);

builder.Services.AddOpenApi();
builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence();

builder.Services.AddHealthChecks()
    .AddNpgSql(provider =>
    {
        var configuration = provider.GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        ArgumentException.ThrowIfNullOrEmpty(connectionString, "Connection string 'DefaultConnection' not found.");

        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        ArgumentException.ThrowIfNullOrEmpty(dbHost, "Database host 'DB_HOST' not found.");
        var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
        ArgumentException.ThrowIfNullOrEmpty(dbPort, "Database port 'DB_PORT' not found.");
        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        ArgumentException.ThrowIfNullOrEmpty(dbUser, "Database user 'DB_USER' not found.");
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        ArgumentException.ThrowIfNullOrEmpty(dbPassword, "Database password 'DB_PASSWORD' not found.");

        connectionString = connectionString
            .Replace("#{DB_HOST}", dbHost)
            .Replace("#{DB_PORT}", dbPort)
            .Replace("#{DB_USER}", dbUser)
            .Replace("#{DB_PASSWORD}", dbPassword);
        return connectionString;
    });

builder.Services.AddTransient<GlobalExceptionHandler>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

// Configure the HTTP request pipeline.
if ((app.Configuration["ASPNETCORE_ENVIRONMENT"] ?? "Production") == "Development")
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/healthz");

app.MapGet("/api/users/{userId}/tasks", async (Guid userId, IMediator mediator, CancellationToken cancellationToken) =>
{
    var query = new GetTasksByUserIdQuery(userId);
    var result = await mediator.Send(query, cancellationToken);
    return Results.Ok(result);
})
.WithName("GetTasksByUserId");

await app.RunAsync();