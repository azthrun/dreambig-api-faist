using Microsoft.Extensions.DependencyInjection;

namespace DreamBig.Faist.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR, FluentValidation, or mapping profiles here later.
        return services;
    }
}
