using Domain.Reviews;

namespace Infrastructure.Repositories;

internal sealed class ReviewRepository : Repository<Review, ReviewId>, IReviewRepository
{
    public ReviewRepository(
        ApplicationDbContext dbContext) 
        : base(dbContext)
    {
    }
}
