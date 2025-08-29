
using DreamBig.Faist.Application.Common.Behaviors;
using DreamBig.Faist.Application.Common.Extensions;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace DreamBig.Faist.UnitTests.Application.Common.Extensions;

public class DependencyInjectionTests
{
    [Fact(DisplayName = "AddApplication should register Mediator with a scoped lifetime")]
    public void AddApplication_Should_Register_Mediator()
    {
        // Arrange
        ServiceCollection services = new();

        // Act
        services.AddApplication();

        // Assert
        ServiceDescriptor? mediatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IMediator));
        mediatorDescriptor.ShouldNotBeNull();
        mediatorDescriptor.Lifetime.ShouldBe(ServiceLifetime.Scoped);
    }

    [Fact(DisplayName = "AddApplication should register ValidationBehavior with a scoped lifetime")]
    public void AddApplication_Should_Register_ValidationBehavior()
    {
        // Arrange
        ServiceCollection services = new();

        // Act
        services.AddApplication();

        // Assert
        ServiceDescriptor? validationBehaviorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IPipelineBehavior<,>));
        validationBehaviorDescriptor.ShouldNotBeNull();
        validationBehaviorDescriptor.ImplementationType.ShouldBe(typeof(ValidationBehavior<,>));
        validationBehaviorDescriptor.Lifetime.ShouldBe(ServiceLifetime.Scoped);
    }
}
