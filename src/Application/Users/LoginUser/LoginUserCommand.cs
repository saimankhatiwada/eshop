using Application.Abstractions.Messaging;

namespace Application.Users.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password) : ICommand<AccessTokenResponse>;