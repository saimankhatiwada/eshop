using Application.Products.CreateProduct;
using Application.Products.DeleteProduct;
using Application.Products.GetAllProduct;
using Application.Products.GetProduct;
using Application.Products.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Product;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(
            request.Name,
            request.Description,
            request.Amount,
            request.Currency,
            request.Quantity);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? BadRequest(result.Error) 
            : CreatedAtAction(nameof(GetProduct), new { id = result.Value }, result.Value);
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

    [HttpGet]
    public async Task<IActionResult> GetAllProduct(
        CancellationToken cancellationToken)
    {
        var query = new GetAllProductQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(
        UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(
            request.ProductId,
            request.Name,
            request.Description,
            request.Amount,
            request.Currency,
            request.Quantity);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
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
