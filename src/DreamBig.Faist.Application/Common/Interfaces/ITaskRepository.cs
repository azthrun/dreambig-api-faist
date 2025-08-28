using Task = DreamBig.Faist.Domain.Entities.Task;

namespace DreamBig.Faist.Application.Common.Interfaces;

public interface ITaskRepository : IRepository<Task>
{
    Task<IEnumerable<Task>> GetTasksByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
