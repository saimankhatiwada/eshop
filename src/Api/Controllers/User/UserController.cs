using Application.Users.LoginUser;
using Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(
        ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

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
