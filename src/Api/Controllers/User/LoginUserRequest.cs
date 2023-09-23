namespace Api.Controllers.User;

public sealed record LoginUserRequest(
    string Email,
    string Password);
