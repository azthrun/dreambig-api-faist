using DreamBig.Faist.Persistence;
using DreamBig.Faist.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Task = DreamBig.Faist.Domain.Entities.Task;

namespace DreamBig.Faist.UnitTests.Persistence.Repositories;

public sealed class RepositoryTests
{
    private readonly FaistDbContext _context;
    private readonly Repository<Task> _repository;

    public RepositoryTests()
    {
        DbContextOptions<FaistDbContext> options = new DbContextOptionsBuilder<FaistDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new FaistDbContext(options);
        _repository = new Repository<Task>(_context);
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

    [Fact(DisplayName = "GetByIdAsync should return the entity when it exists")]
    public async System.Threading.Tasks.Task GetByIdAsync_Should_Return_Entity_When_Exists()
    {
        // Arrange
        await SeedDataAsync();
        Task task = await _context.Tasks.FirstAsync();

        // Act
        Task? result = await _repository.GetByIdAsync(task.Id, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(task.Id);
    }

    [Fact(DisplayName = "GetByIdAsync should return null when the entity does not exist")]
    public async System.Threading.Tasks.Task GetByIdAsync_Should_Return_Null_When_Not_Exists()
    {
        // Arrange
        await SeedDataAsync();

        // Act
        Task? result = await _repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.ShouldBeNull();
    }

    [Fact(DisplayName = "GetAllAsync should return all entities")]
    public async System.Threading.Tasks.Task GetAllAsync_Should_Return_All_Entities()
    {
        // Arrange
        await SeedDataAsync();

        // Act
        IEnumerable<Task> result = await _repository.GetAllAsync(CancellationToken.None);

        // Assert
        result.Count().ShouldBe(3);
    }

    [Fact(DisplayName = "FindAsync should return matching entities")]
    public async System.Threading.Tasks.Task FindAsync_Should_Return_Matching_Entities()
    {
        // Arrange
        await SeedDataAsync();
        Guid userId = _context.Tasks.First().UserId;

        // Act
        IEnumerable<Task> result = await _repository.FindAsync(t => t.UserId == userId, CancellationToken.None);

        // Assert
        result.Count().ShouldBe(2);
    }

    [Fact(DisplayName = "AddAsync should add the entity to the context")]
    public async System.Threading.Tasks.Task AddAsync_Should_Add_Entity_To_Context()
    {
        // Arrange
        Task task = new()
        { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Title = "New Task" };

        // Act
        await _repository.AddAsync(task, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        Task? result = await _context.Tasks.FindAsync(task.Id);
        result.ShouldNotBeNull();
    }

    [Fact(DisplayName = "Update should update the entity in the context")]
    public async System.Threading.Tasks.Task Update_Should_Update_Entity_In_Context()
    {
        // Arrange
        await SeedDataAsync();
        Task task = await _context.Tasks.FirstAsync();
        task.Title = "Updated Title";

        // Act
        _repository.Update(task);
        await _context.SaveChangesAsync();

        // Assert
        Task? result = await _context.Tasks.FindAsync(task.Id);
        result.ShouldNotBeNull();
        result.Title.ShouldBe("Updated Title");
    }

    [Fact(DisplayName = "Remove should remove the entity from the context")]
    public async System.Threading.Tasks.Task Remove_Should_Remove_Entity_From_Context()
    {
        // Arrange
        await SeedDataAsync();
        Task task = await _context.Tasks.FirstAsync();

        // Act
        _repository.Remove(task);
        await _context.SaveChangesAsync();

        // Assert
        Task? result = await _context.Tasks.FindAsync(task.Id);
        result.ShouldBeNull();
    }
}
