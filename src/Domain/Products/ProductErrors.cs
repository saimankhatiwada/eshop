using Domain.Abstractions;

namespace Domain.Products;

public static class ProductErrors
{
    public static Error NotFound = new(
        "Product.NotFound",
        "The product with the specified identifier was not found");
}