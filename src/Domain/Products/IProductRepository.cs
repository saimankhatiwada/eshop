namespace Domain.Products;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default);

    void Add(Product product);

    void Delete(Product product);

}
