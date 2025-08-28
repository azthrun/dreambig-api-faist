using DreamBig.Faist.Application.Common.Interfaces;
using DreamBig.Faist.Application.Tasks.Dtos;
using Mediator;

namespace DreamBig.Faist.Application.Tasks.Queries;

public sealed class GetTasksByUserIdQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetTasksByUserIdQuery, IEnumerable<TaskDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async ValueTask<IEnumerable<TaskDto>> Handle(GetTasksByUserIdQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Task> tasks = await _unitOfWork.Tasks.GetTasksByUserIdAsync(query.UserId, cancellationToken);

        IEnumerable<TaskDto> taskDtos = tasks.Select(task => new TaskDto
        {
            Id = task.Id,
            UserId = task.UserId,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted
        });

        return taskDtos;
    }
}
