using DreamBig.Faist.Application;
using DreamBig.Faist.Application.Tasks.Queries;
using DreamBig.Faist.Infrastructure;
using DreamBig.Faist.Persistence;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(builder.Configuration);

builder.Services.AddValidatorsFromAssembly(typeof(GetTasksByUserIdQuery).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/users/{userId}/tasks", async (Guid userId, IMediator mediator) =>
{
    var query = new GetTasksByUserIdQuery(userId);
    var result = await mediator.Send(query);
    return Results.Ok(result);
})
.WithName("GetTasksByUserId");

app.Run();
