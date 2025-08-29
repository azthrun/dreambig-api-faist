using System.Text.Json;
using DreamBig.Faist.Api.Middleware;
using DreamBig.Faist.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace DreamBig.Faist.UnitTests.Api.Middleware;

public class GlobalExceptionHandlerTests
{
    private readonly GlobalExceptionHandler _handler;

    public GlobalExceptionHandlerTests()
    {
        _handler = new GlobalExceptionHandler();
    }

    [Fact(DisplayName = "InvokeAsync should handle RetriableException")]
    public async Task InvokeAsync_Should_Handle_RetriableException()
    {
        // Arrange
        DefaultHttpContext context = new();
        context.Response.Body = new MemoryStream();
        RetriableException exception = new("Test retriable exception");
        RequestDelegate next = (ctx) => throw exception;

        // Act
        await _handler.InvokeAsync(context, next);

        // Assert
        context.Response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        context.Response.ContentType.ShouldBe("application/problem+json");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using StreamReader reader = new(context.Response.Body);
        string body = await reader.ReadToEndAsync();
        ProblemDetails? problemDetails = JsonSerializer.Deserialize<ProblemDetails>(body);

        problemDetails.ShouldNotBeNull();
        problemDetails.Title.ShouldBe("A retriable error occurred");
        problemDetails.Status.ShouldBe(StatusCodes.Status400BadRequest);
        problemDetails.Detail.ShouldBe(exception.Message);
    }

    [Fact(DisplayName = "InvokeAsync should handle generic Exception")]
    public async Task InvokeAsync_Should_Handle_Generic_Exception()
    {
        // Arrange
        DefaultHttpContext context = new();
        context.Response.Body = new MemoryStream();
        Exception exception = new("Test generic exception");
        RequestDelegate next = (ctx) => throw exception;

        // Act
        await _handler.InvokeAsync(context, next);

        // Assert
        context.Response.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        context.Response.ContentType.ShouldBe("application/problem+json");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using StreamReader reader = new(context.Response.Body);
        string body = await reader.ReadToEndAsync();
        ProblemDetails? problemDetails = JsonSerializer.Deserialize<ProblemDetails>(body);

        problemDetails.ShouldNotBeNull();
        problemDetails.Title.ShouldBe("An unexpected error occurred");
        problemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        problemDetails.Detail.ShouldBe(exception.Message);
    }

    [Fact(DisplayName = "InvokeAsync should not handle exception when no exception is thrown")]
    public async Task InvokeAsync_Should_Not_Handle_Exception_When_No_Exception_Is_Thrown()
    {
        // Arrange
        DefaultHttpContext context = new();
        bool called = false;
        RequestDelegate next = (ctx) => 
        {
            called = true;
            return System.Threading.Tasks.Task.CompletedTask;
        };

        // Act
        await _handler.InvokeAsync(context, next);

        // Assert
        called.ShouldBeTrue();
        context.Response.StatusCode.ShouldBe(StatusCodes.Status200OK);
    }
}