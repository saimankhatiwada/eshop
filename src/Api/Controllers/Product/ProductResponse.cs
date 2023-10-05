namespace Api.Controllers.Product;

public record ProductResponse(
    Guid ProductId,
    string Name,
    string ImageUrl,
    string Description,
    decimal Amount,
    string Currency,
    int Quantity);
