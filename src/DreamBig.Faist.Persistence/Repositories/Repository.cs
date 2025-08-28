using System.Linq.Expressions;
using DreamBig.Faist.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DreamBig.Faist.Persistence.Repositories;

public class Repository<T>(
    FaistDbContext context
) : IRepository<T> where T : class
{
    protected readonly FaistDbContext _context = context;

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => await _context.Set<T>().FindAsync([id], cancellationToken);

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken) => await _context.Set<T>().ToListAsync(cancellationToken);

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) => await _context.Set<T>().Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken) => await _context.Set<T>().AddAsync(entity, cancellationToken);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void Remove(T entity) => _context.Set<T>().Remove(entity);
}
