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

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence();

builder.Services.AddScoped<IValidator<GetTasksByUserIdQuery>, GetTasksByUserIdQueryValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if ((app.Configuration["ASPNETCORE_ENVIRONMENT"] ?? "Production") == "Development")
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/users/{userId}/tasks", async (Guid userId, IMediator mediator, CancellationToken cancellationToken) =>
{
    var query = new GetTasksByUserIdQuery(userId);
    var result = await mediator.Send(query, cancellationToken);
    return Results.Ok(result);
})
.WithName("GetTasksByUserId");

await app.RunAsync();