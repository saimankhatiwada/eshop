using Application.Abstractions.Messaging;

namespace Application.Products.UpdateProduct;
public record UpdateProductCommand(
    Guid ProductId,
    string Name,
    string Description,
    decimal Amount,
    string Currency,
    int Quantity) : ICommand;
