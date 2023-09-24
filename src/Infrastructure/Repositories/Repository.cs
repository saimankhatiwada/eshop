using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal abstract class Repository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : class
{
    private readonly ApplicationDbContext _dbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity?> GetByIdAsync(
        TEntityId id,
        CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<TEntity>()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public void Add(TEntity entity)
    {
        _dbContext.Add(entity);
    }
    
    public void Delete(TEntity entity)
    {
        _dbContext.Remove(entity);
    }
}
