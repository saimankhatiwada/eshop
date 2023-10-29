namespace Api.Controllers.Shared.Product;

public sealed record CreateProductRequest(
    string Name,
    string Description,
    decimal Amount,
    string Currency,
    int Quantity,
    IFormFile File);
