using DreamBig.Faist.Application.Common.Interfaces;

namespace DreamBig.Faist.Persistence;

public sealed class UnitOfWork(
    FaistDbContext context,
    ITaskRepository taskRepository
) : IUnitOfWork
{
    private readonly FaistDbContext _context = context;
    public ITaskRepository Tasks { get; } = taskRepository;

    public async Task<int> CompleteAsync(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
