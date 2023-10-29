using Application.Abstractions.Messaging;

namespace Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ImageName,
    string FileContentType,
    Stream FileStream) : ICommand<Guid>;
