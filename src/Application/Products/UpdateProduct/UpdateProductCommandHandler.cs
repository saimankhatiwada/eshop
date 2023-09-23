using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(
        UpdateProductCommand request, 
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(new ProductId(request.ProductId), cancellationToken);

        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound);
        }

        var result = product?.Update(
            new Name(request.Name),
            new Description(request.Description),
            new Money(request.Amount, Currency.FromCode(request.Currency)),
            Quantity.Create(request.Quantity).Value);

        if (result!.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
