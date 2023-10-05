namespace Application.Products.Shared;

#pragma warning disable CS8618
public sealed class ProductResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ImageName { get; init; }
    public string Description { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    public int Quantity { get; init; }
}

#pragma warning restore CS8618