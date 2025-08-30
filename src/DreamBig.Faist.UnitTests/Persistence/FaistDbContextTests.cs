using DreamBig.Faist.Persistence;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Task = DreamBig.Faist.Domain.Entities.Task;

namespace DreamBig.Faist.UnitTests.Persistence;

public sealed class FaistDbContextTests
{
    private readonly FaistDbContext _context;

    public FaistDbContextTests()
    {
        DbContextOptions<FaistDbContext> options = new DbContextOptionsBuilder<FaistDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new FaistDbContext(options);
    }

    [Fact(DisplayName = "SaveChangesAsync should set CreatedAt and UpdatedAt on add")]
    public async System.Threading.Tasks.Task SaveChangesAsync_Should_Set_CreatedAt_And_UpdatedAt_On_Add()
    {
        // Arrange
        Task task = new()
        { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Title = "New Task" };
        _context.Tasks.Add(task);

        // Act
        await _context.SaveChangesAsync();

        // Assert
        task.CreatedAt.ShouldNotBe(default);
        task.UpdatedAt.ShouldNotBe(default);
    }

    [Fact(DisplayName = "SaveChangesAsync should set UpdatedAt on update")]
    public async System.Threading.Tasks.Task SaveChangesAsync_Should_Set_UpdatedAt_On_Update()
    {
        // Arrange
        Task task = new()
        { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Title = "New Task" };
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        DateTime createdAt = task.CreatedAt;
        DateTime updatedAt = task.UpdatedAt;

        task.Title = "Updated Title";
        await System.Threading.Tasks.Task.Delay(10); // Ensure UpdatedAt will be different

        // Act
        await _context.SaveChangesAsync();

        // Assert
        task.UpdatedAt.ShouldBeGreaterThan(updatedAt);
        task.CreatedAt.ShouldBe(createdAt);
    }
}