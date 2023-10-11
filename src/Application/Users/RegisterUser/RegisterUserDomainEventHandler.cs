using System.Text;
using Application.Abstractions.Email;
using Domain.Users;
using Domain.Users.Events;
using MediatR;

namespace Application.Users.RegisterUser;

internal sealed class RegisterUserDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public RegisterUserDomainEventHandler(
        IUserRepository userRepository,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task Handle(
        UserCreatedDomainEvent notification, 
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);

        if (user is null)
        {
            return;
        }

        string htmlTemplate = """
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="UTF-8" />
                <title>Welcome to Our eshop</title>
                <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f5f5f5;
                    margin: 0;
                    padding: 0;
                }
                .container {
                    background-color: #ffffff;
                    max-width: 600px;
                    margin: 20px auto;
                    padding: 20px;
                    border-radius: 5px;
                    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                }
                h1 {
                    color: #333333;
                }
                p {
                    color: #666666;
                }
                </style>
            </head>
            <body>
                <div class="container">
                <h1>Welcome to Our online platform</h1>
                <p>Dear [USER_NAME],</p>
                <p>
                    Thank you for registering on our website. We are excited to have you as
                    a member of our community.
                </p>
                <p>
                    You can now enjoy all the features and benefits our platform has to
                    offer.
                </p>
                <p>
                    If you have any questions or need assistance, feel free to contact our
                    support team.
                </p>
                <p>Best regards,</p>
                <p>eshop</p>
                </div>
            </body>
            </html>

        """;

        string htmlMessage = new StringBuilder(htmlTemplate)
            .Replace("[USER_NAME]", $"{user.FirstName.Value} {user.LastName.Value}")
            .ToString();

        const string subject = "Welcome to eshop";

        await _emailService.SendAsync(user, subject, htmlMessage);
    }
}
