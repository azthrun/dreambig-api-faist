using DreamBig.Faist.Application.Common.Interfaces;
using DreamBig.Faist.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace DreamBig.Faist.UnitTests.Persistence;

public class DependencyInjectionTests
{
    private readonly IConfiguration _configuration;

    public DependencyInjectionTests()
    {
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", "Host=#{DB_HOST};Port=#{DB_PORT};Username=#{DB_USER};Password=#{DB_PASSWORD};Database=Faist")
            ])
            .Build();

        Environment.SetEnvironmentVariable("DB_HOST", "localhost");
        Environment.SetEnvironmentVariable("DB_PORT", "5432");
        Environment.SetEnvironmentVariable("DB_USER", "postgres");
        Environment.SetEnvironmentVariable("DB_PASSWORD", "postgres");
    }

    [Fact(DisplayName = "AddPersistence should register FaistDbContext")]
    public void AddPersistence_Should_Register_FaistDbContext()
    {
        // Arrange
        ServiceCollection services = new();
        services.AddSingleton(_configuration);

        // Act
        services.AddPersistence();

        // Assert
        ServiceDescriptor? dbContextDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(FaistDbContext));
        dbContextDescriptor.ShouldNotBeNull();
    }

    [Fact(DisplayName = "AddPersistence should register repositories and UnitOfWork with a scoped lifetime")]
    public void AddPersistence_Should_Register_Repositories_And_UnitOfWork()
    {
        // Arrange
        ServiceCollection services = new();
        services.AddSingleton(_configuration);

        // Act
        services.AddPersistence();

        // Assert
        ServiceDescriptor? taskRepositoryDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ITaskRepository));
        taskRepositoryDescriptor.ShouldNotBeNull();
        taskRepositoryDescriptor.Lifetime.ShouldBe(ServiceLifetime.Scoped);

        ServiceDescriptor? unitOfWorkDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IUnitOfWork));
        unitOfWorkDescriptor.ShouldNotBeNull();
        unitOfWorkDescriptor.Lifetime.ShouldBe(ServiceLifetime.Scoped);
    }

    [Fact(DisplayName = "AddPersistence should throw an exception when the connection string is null")]
    public void AddPersistence_Should_Throw_Exception_When_Connection_String_Is_Null()
    {
        // Arrange
        ServiceCollection services = new();
        IConfigurationRoot configuration = new ConfigurationBuilder().Build();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddPersistence();

        // Act & Assert
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        Should.Throw<ArgumentException>(() => serviceProvider.GetRequiredService<FaistDbContext>());
    }

    [Fact(DisplayName = "AddPersistence should throw an exception when the connection string is empty")]
    public void AddPersistence_Should_Throw_Exception_When_Connection_String_Is_Empty()
    {
        // Arrange
        ServiceCollection services = new();
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", "")
            ])
            .Build();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddPersistence();

        // Act & Assert
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        Should.Throw<ArgumentException>(() => serviceProvider.GetRequiredService<FaistDbContext>());
    }

    [Fact(DisplayName = "AddPersistence should throw an exception when DB_HOST environment variable is missing")]
    public void AddPersistence_Should_Throw_Exception_When_DB_HOST_Is_Missing()
    {
        // Arrange
        ServiceCollection services = new();
        services.AddSingleton(_configuration);
        Environment.SetEnvironmentVariable("DB_HOST", null);
        services.AddPersistence();

        // Act & Assert
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        Should.Throw<ArgumentException>(() => serviceProvider.GetRequiredService<FaistDbContext>());
    }
}