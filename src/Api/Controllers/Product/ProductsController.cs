using Application.Abstractions.Storage;
using Application.Products.CreateProduct;
using Application.Products.DeleteProduct;
using Application.Products.GetAllProduct;
using Application.Products.GetProduct;
using Application.Products.UpdateProduct;
using Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Product;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IStorageService _storageService;
    private readonly S3BucketOptions _s3BucketOptions;
    public ProductsController(
        ISender sender,
        IStorageService storageService,
        IOptions<S3BucketOptions> s3BucketOptions)
    {
        _sender = sender;
        _storageService = storageService;
        _s3BucketOptions = s3BucketOptions.Value;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct(
        [FromForm]CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        FileInfo fileInfo = new(request.File.FileName);
        string fileNameWithPrefix = $"{_s3BucketOptions.Products}/{Guid.NewGuid().ToString() + fileInfo.Extension}";

        var command = new CreateProductCommand(
            request.Name,
            fileNameWithPrefix,
            request.Description,
            request.Amount,
            request.Currency,
            request.Quantity);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            await _storageService.UploadFileAsync(fileNameWithPrefix, request.File.ContentType, request.File.OpenReadStream());
            return CreatedAtAction(nameof(GetProduct), new { id = result.Value }, result.Value);
        } 

        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetProductQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            var response = new ProductResponse(
            result.Value.Id,
            result.Value.Name,
            _storageService.GetPreSignedUrlAsync(result.Value.ImageName).Result.Value,
            result.Value.Description,
            result.Value.Amount,
            result.Value.Currency,
            result.Value.Quantity);

            return Ok(response);
        }

        return NotFound();
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAllProduct(
        CancellationToken cancellationToken)
    {
        var query = new GetAllProductQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateProduct(
        [FromForm]UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        FileInfo fileInfo = new(request.File.FileName);
        string fileNameWithPrefix = $"{_s3BucketOptions.Products}/{Guid.NewGuid().ToString() + fileInfo.Extension}";

        var command = new UpdateProductCommand(
            request.ProductId,
            request.Name,
            fileNameWithPrefix,
            request.Description,
            request.Amount,
            request.Currency,
            request.Quantity);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            await _storageService.UploadFileAsync(fileNameWithPrefix, request.File.ContentType, request.File.OpenReadStream());
            return Ok();
        }

        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
