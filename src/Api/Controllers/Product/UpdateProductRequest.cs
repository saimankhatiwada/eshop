namespace Api.Controllers.Product;

public sealed record UpdateProductRequest(
    Guid ProductId,
    string Name,
    string Description,
    decimal Amount,
    string Currency,
    int Quantity,
    IFormFile File);
