using Application.Abstractions.Messaging;

namespace Application.Products.UpdateProduct;
public record UpdateProductCommand(
    Guid ProductId,
    string Name,
    string ImageName,
    string Description,
    decimal Amount,
    string Currency,
    int Quantity) : ICommand;
