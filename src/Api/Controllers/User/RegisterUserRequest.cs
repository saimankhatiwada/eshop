namespace Api.Controllers.User;

public sealed record RegisterUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    IFormFile File);
