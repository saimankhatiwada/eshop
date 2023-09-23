namespace Domain.Products;

public record ProductId(Guid value)
{
    public static ProductId New() => new(Guid.NewGuid());
}
