using Application.Products.GetAllProduct;
using Application.Products.GetProduct;
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
    private readonly S3BucketOptions _s3BucketOptions;
    public ProductsController(
        ISender sender,
        IOptions<S3BucketOptions> s3BucketOptions)
    {
        _sender = sender;
        _s3BucketOptions = s3BucketOptions.Value;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetProductQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAllProduct(
        double? greaterThan,
        double? lessThan,
        string? sortColumn,
        string? sortOrder,
        CancellationToken cancellationToken)
    {
        var query = new GetAllProductQuery(greaterThan, lessThan, sortColumn, sortOrder);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NoContent();
    }
}
