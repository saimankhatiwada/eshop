namespace Api.Controllers.Product;

public sealed record CreateProductRequest(
    string Name,
    string Description,
    decimal Amount,
    string Currency,
    int Quantity);
