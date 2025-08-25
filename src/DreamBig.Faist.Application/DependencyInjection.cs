using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace DreamBig.Faist.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator();
        return services;
    }
}
