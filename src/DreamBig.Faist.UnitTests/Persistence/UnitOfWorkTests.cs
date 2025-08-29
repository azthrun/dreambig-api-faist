using DreamBig.Faist.Application.Common.Interfaces;
using DreamBig.Faist.Persistence;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Shouldly;
using Xunit;
using Task = DreamBig.Faist.Domain.Entities.Task;

namespace DreamBig.Faist.UnitTests.Persistence;

public class UnitOfWorkTests
{
    private readonly FaistDbContext _context;
    private readonly ITaskRepository _taskRepository;
    private readonly UnitOfWork _unitOfWork;

    public UnitOfWorkTests()
    {
        DbContextOptions<FaistDbContext> options = new DbContextOptionsBuilder<FaistDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new FaistDbContext(options);
        _taskRepository = Substitute.For<ITaskRepository>();
        _unitOfWork = new UnitOfWork(_context, _taskRepository);
    }

    [Fact(DisplayName = "CompleteAsync should call SaveChangesAsync on the context and return the number of affected rows")]
    public async System.Threading.Tasks.Task CompleteAsync_Should_Call_SaveChangesAsync_On_Context()
    {
        // Arrange
        Task task = new()
        { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Title = "Test Task", Description = "Test Description" };
        await _context.Tasks.AddAsync(task);

        // Act
        int result = await _unitOfWork.CompleteAsync(CancellationToken.None);

        // Assert
        result.ShouldBe(1);
    }

    [Fact(DisplayName = "Dispose should call Dispose on the context")]
    public void Dispose_Should_Call_Dispose_On_Context()
    {
        // Arrange
        DbContextOptions<FaistDbContext> options = new DbContextOptionsBuilder<FaistDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        FaistDbContext contextMock = Substitute.For<FaistDbContext>(options);
        UnitOfWork unitOfWork = new(contextMock, _taskRepository);

        // Act
        unitOfWork.Dispose();

        // Assert
        contextMock.Received(1).Dispose();
    }
}