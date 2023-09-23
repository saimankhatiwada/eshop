using Domain.Users;

namespace Application.Abstractions.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(
        User user,
        string Password,
        CancellationToken cancellationToken = default);
}
