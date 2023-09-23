using Domain.Products;

namespace Infrastructure.Repositories;

internal sealed class ProductRepository : Repository<Product, ProductId>, IProductRepository
{
    public ProductRepository(
        ApplicationDbContext dbContext) 
        : base(dbContext)
    {
    }

}
