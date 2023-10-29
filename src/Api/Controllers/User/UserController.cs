using Application.Users.LoginUser;
using Application.Users.RegisterUser;
using Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers.User;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ISender _sender;
    private readonly S3BucketOptions _s3BucketOptions;

    public UserController(
        ISender sender,
        IOptions<S3BucketOptions> s3BucketOptions)
    {
        _sender = sender;
        _s3BucketOptions = s3BucketOptions.Value;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromForm]RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        FileInfo fileInfo = new(request.File.FileName);
        string fileNameWithPrefix = $"{_s3BucketOptions.Users}/{Guid.NewGuid().ToString() + fileInfo.Extension}";

        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password,
            fileNameWithPrefix,
            request.File.ContentType,
            request.File.OpenReadStream());

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(
            request.Email,
            request.Password);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
    }
}
