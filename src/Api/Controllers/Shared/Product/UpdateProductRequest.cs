namespace Api.Controllers.Shared.Product;

public sealed record UpdateProductRequest(
    Guid ProductId,
    string Name,
    string Description,
    decimal Amount,
    string Currency,
    int Quantity,
    IFormFile File);
