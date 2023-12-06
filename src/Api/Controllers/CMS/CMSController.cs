using Api.Controllers.Shared.Product;
using Application.Products.CreateProduct;
using Application.Products.DeleteProduct;
using Application.Products.GetAllProduct;
using Application.Products.GetProduct;
using Application.Products.UpdateProduct;
using Application.Users.GetAllUser;
using Application.Users.GetUser;
using Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers.CMS;

[ApiController]
[Route("api/[controller]")]
public class CMSController : ControllerBase
{
    private readonly ISender _sender;
    private readonly S3BucketOptions _s3BucketOptions;

    public CMSController(
        ISender sender,
        IOptions<S3BucketOptions> s3BucketOptions)
    {
        _sender = sender;
        _s3BucketOptions = s3BucketOptions.Value;
    }

    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetUserQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NoContent();
    }

    [HttpGet("users/get")]
    public async Task<IActionResult> GetAllUser(
        CancellationToken cancellationToken)
    {
        var query = new GetAllUserQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NoContent();
    }

    [HttpPost("products/create")]
    public async Task<IActionResult> CreateProduct(
        [FromForm] CreateProductRequest request,
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
            request.Quantity,
            request.File.ContentType,
            request.File.OpenReadStream());

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(GetProduct), new { id = result.Value }, result.Value) : BadRequest(result.Error);
    }

    [HttpGet("products/{id}")]
    public async Task<IActionResult> GetProduct(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetProductQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpGet("products/get")]
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

    [HttpPut("products/update")]
    public async Task<IActionResult> UpdateProduct(
        [FromForm] UpdateProductRequest request,
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
            request.Quantity,
            request.File.ContentType,
            request.File.OpenReadStream());

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpDelete("products/{id}")]
    public async Task<IActionResult> DeleteProduct(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
