namespace DreamBig.Faist.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ITaskRepository Tasks { get; }
    Task<int> CompleteAsync(CancellationToken cancellationToken);
}
