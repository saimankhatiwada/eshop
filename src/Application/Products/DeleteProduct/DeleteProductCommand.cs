using Application.Abstractions.Messaging;

namespace Application.Products.DeleteProduct;
public record DeleteProductCommand(Guid ProductId) : ICommand;
