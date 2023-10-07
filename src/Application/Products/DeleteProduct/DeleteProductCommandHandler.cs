using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Domain.Abstractions;
using Domain.Products;

namespace Application.Products.DeleteProduct;

internal sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(
        IProductRepository productRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(
        DeleteProductCommand request, 
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(new ProductId(request.ProductId), cancellationToken);

        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound);
        }

        var result = product?.Delete();

        if (result!.IsFailure)
        {
            return result;
        }

        await _storageService.DeleteFileAsync(product!.ImageName.Value);

        _productRepository.Delete(product!);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
