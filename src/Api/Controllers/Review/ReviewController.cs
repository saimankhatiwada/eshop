using Application.Reviews.CreateReview;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Review;

[ApiController]
[Route("api/reviews")]
public class ReviewController : ControllerBase
{
    private readonly ISender _sender;

    public ReviewController(
        ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateReview(
        CreateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateReviewCommand(
            request.ProductId,
            request.UserId,
            request.Rating,
            request.Comment);

        var result = await _sender.Send(command, cancellationToken);

        return Ok(result.Value);
    }
}
