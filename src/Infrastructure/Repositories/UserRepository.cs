using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    public UserRepository(
        ApplicationDbContext dbContext) 
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByEmailAsync(Domain.Users.Email email, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<User>()
            .Where(entity => entity.Email == email)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
