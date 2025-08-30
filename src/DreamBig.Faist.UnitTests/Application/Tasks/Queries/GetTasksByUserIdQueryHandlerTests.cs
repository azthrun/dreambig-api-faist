
using DreamBig.Faist.Application.Common.Interfaces;
using DreamBig.Faist.Application.Tasks.Dtos;
using DreamBig.Faist.Application.Tasks.Queries;
using NSubstitute;
using Shouldly;
using Xunit;
using Task = DreamBig.Faist.Domain.Entities.Task;

namespace DreamBig.Faist.UnitTests.Application.Tasks.Queries;

public sealed class GetTasksByUserIdQueryHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetTasksByUserIdQueryHandler _handler;

    public GetTasksByUserIdQueryHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new GetTasksByUserIdQueryHandler(_unitOfWork);
    }

    [Fact(DisplayName = "Handle should return TaskDtos when tasks exist")]
    public async System.Threading.Tasks.Task Handle_Should_Return_TaskDtos_When_Tasks_Exist()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        List<Task> tasks =
        [
            new Task { Id = Guid.NewGuid(), UserId = userId, Title = "Task 1", Description = "Description 1", IsCompleted = false },
            new Task { Id = Guid.NewGuid(), UserId = userId, Title = "Task 2", Description = "Description 2", IsCompleted = true }
        ];
        GetTasksByUserIdQuery query = new(userId);

        _unitOfWork.Tasks.GetTasksByUserIdAsync(userId, Arg.Any<CancellationToken>()).Returns(tasks);

        // Act
        IEnumerable<TaskDto> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeAssignableTo<IEnumerable<TaskDto>>();
        result.Count().ShouldBe(2);
        result.First().Title.ShouldBe("Task 1");
        result.Last().Title.ShouldBe("Task 2");
    }

    [Fact(DisplayName = "Handle should return an empty list when no tasks exist")]
    public async System.Threading.Tasks.Task Handle_Should_Return_Empty_List_When_No_Tasks_Exist()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        GetTasksByUserIdQuery query = new(userId);

        _unitOfWork.Tasks.GetTasksByUserIdAsync(userId, Arg.Any<CancellationToken>()).Returns([]);

        // Act
        IEnumerable<TaskDto> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }
}
