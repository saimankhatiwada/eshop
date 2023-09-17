using Application.Abstractions.Email;

namespace Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync()
    {
        //ToDo
        return Task.CompletedTask;
    }
}
