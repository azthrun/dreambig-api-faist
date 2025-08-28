using DreamBig.Faist.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskEntity = DreamBig.Faist.Domain.Entities.Task;

namespace DreamBig.Faist.Persistence.Repositories;

public sealed class TaskRepository(
    FaistDbContext context
) : Repository<TaskEntity>(context), ITaskRepository
{

    public async Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}
