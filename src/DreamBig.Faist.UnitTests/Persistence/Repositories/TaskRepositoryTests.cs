using DreamBig.Faist.Persistence;
using DreamBig.Faist.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Task = DreamBig.Faist.Domain.Entities.Task;

namespace DreamBig.Faist.UnitTests.Persistence.Repositories;

public class TaskRepositoryTests
{
    private readonly FaistDbContext _context;
    private readonly TaskRepository _repository;

    public TaskRepositoryTests()
    {
        DbContextOptions<FaistDbContext> options = new DbContextOptionsBuilder<FaistDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new FaistDbContext(options);
        _repository = new TaskRepository(_context);
    }

    private async System.Threading.Tasks.Task SeedDataAsync()
    {
        Guid userId1 = Guid.NewGuid();
        Guid userId2 = Guid.NewGuid();

        List<Task> tasks =

        [
            new Task { Id = Guid.NewGuid(), UserId = userId1, Title = "Task 1", Description = "Description 1", IsCompleted = false },
            new Task { Id = Guid.NewGuid(), UserId = userId1, Title = "Task 2", Description = "Description 2", IsCompleted = true },
            new Task { Id = Guid.NewGuid(), UserId = userId2, Title = "Task 3", Description = "Description 3", IsCompleted = false }
        ];

        await _context.Tasks.AddRangeAsync(tasks);
        await _context.SaveChangesAsync();
    }

    [Fact(DisplayName = "GetTasksByUserIdAsync should return tasks for a specific user")]
    public async System.Threading.Tasks.Task GetTasksByUserIdAsync_Should_Return_Tasks_For_User()
    {
        // Arrange
        await SeedDataAsync();
        Guid userId = _context.Tasks.First().UserId;

        // Act
        IEnumerable<Task> result = await _repository.GetTasksByUserIdAsync(userId, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result.All(t => t.UserId == userId).ShouldBeTrue();
    }

    [Fact(DisplayName = "GetTasksByUserIdAsync should return an empty list for a user with no tasks")]
    public async System.Threading.Tasks.Task GetTasksByUserIdAsync_Should_Return_Empty_List_For_User_With_No_Tasks()
    {
        // Arrange
        await SeedDataAsync();
        Guid userId = Guid.NewGuid();

        // Act
        IEnumerable<Task> result = await _repository.GetTasksByUserIdAsync(userId, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }
}