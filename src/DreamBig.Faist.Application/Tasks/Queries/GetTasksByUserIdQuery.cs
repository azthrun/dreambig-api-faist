using DreamBig.Faist.Application.Dtos;
using DreamBig.Faist.Domain.Entities;
using Mediator;
using Task = DreamBig.Faist.Domain.Entities.Task;

namespace DreamBig.Faist.Application.Tasks.Queries;

public sealed record GetTasksByUserIdQuery(Guid UserId) : IQuery<IEnumerable<TaskDto>>;

public sealed class GetTasksByUserIdQueryHandler : IQueryHandler<GetTasksByUserIdQuery, IEnumerable<TaskDto>>
{
    public ValueTask<IEnumerable<TaskDto>> Handle(GetTasksByUserIdQuery query, CancellationToken cancellationToken)
    {
        // For now, we will return a dummy list of tasks.
        // Later, we will implement the actual logic to fetch tasks from the database.
        var tasks = new List<Task>
        {
            new Task { Id = Guid.NewGuid(), UserId = query.UserId, Title = "Task 1", Description = "Description 1", IsCompleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Task { Id = Guid.NewGuid(), UserId = query.UserId, Title = "Task 2", Description = "Description 2", IsCompleted = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Task { Id = Guid.NewGuid(), UserId = query.UserId, Title = "Task 3", Description = "Description 3", IsCompleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        };

        var taskDtos = tasks.Select(task => new TaskDto
        {
            Id = task.Id,
            UserId = task.UserId,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted
        });

        return new ValueTask<IEnumerable<TaskDto>>(taskDtos);
    }
}
