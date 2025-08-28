using System.Text.Json;
using DreamBig.Faist.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DreamBig.Faist.Api.Middleware;

public sealed class GlobalExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";
        ProblemDetails problemDetails = new()
        {
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case RetriableException retriableException:
                problemDetails.Title = "A retriable error occurred";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = retriableException.Message;
                break;
            default:
                problemDetails.Title = "An unexpected error occurred";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Detail = exception.Message;
                break;
        }

        string result = JsonSerializer.Serialize(problemDetails);
        return context.Response.WriteAsync(result);
    }
}
