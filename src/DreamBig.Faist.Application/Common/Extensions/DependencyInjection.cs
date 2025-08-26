using DreamBig.Faist.Application.Common.Behaviors;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace DreamBig.Faist.Application.Common.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}
