using DreamBig.Faist.Application.Common.Interfaces;

namespace DreamBig.Faist.Persistence;

public class UnitOfWork(
    FaistDbContext context,
    ITaskRepository taskRepository
) : IUnitOfWork
{
    private readonly FaistDbContext _context = context;
    public ITaskRepository Tasks { get; } = taskRepository;

    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
