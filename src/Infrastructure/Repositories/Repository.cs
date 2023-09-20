using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal abstract class Repository<T>
    where T : Entity
{
    private readonly ApplicationDbContext _dbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<T>()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public void Add(T entity)
    {
        _dbContext.Add(entity);
    }

    public void Update(T entity)
    {
        _dbContext.Update(entity);
    }
    
    public void Delete(T entity)
    {
        _dbContext.Remove(entity);
    }
}
