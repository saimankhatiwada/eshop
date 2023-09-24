using Domain.Users;

namespace Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(User user, string subject, string htmlMessage, CancellationToken cancellationToken = default);
}
