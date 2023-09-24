using Application.Abstractions.Email;
using Domain.Users;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    private readonly MailOptions _mailOptions;

    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IOptions<MailOptions> mailOptions,
        ILogger<EmailService> logger)
    {
        _mailOptions = mailOptions.Value;
        _logger = logger;
    }

    public async Task SendAsync(
        User user, 
        string subject, 
        string htmlMessage, 
        CancellationToken cancellationToken)
    {
        try
        {
            using var email = new MimeMessage
            {
                From = {MailboxAddress.Parse(_mailOptions.MailHostUsername)},
                To = {MailboxAddress.Parse(user.Email.Value)},
                Subject = subject,
                Body = new TextPart(MimeKit.Text.TextFormat.Html) {Text = htmlMessage}
            };

            using var emailClient = new SmtpClient();

            await emailClient.ConnectAsync(
                _mailOptions.MailHost, 
                _mailOptions.MailHostPort, 
                MailKit.Security.SecureSocketOptions.StartTls, 
                cancellationToken);

            await emailClient.AuthenticateAsync(
                _mailOptions.MailHostUsername, 
                _mailOptions.MailHostSecretKey, 
                cancellationToken);

            await emailClient.SendAsync(
                email, 
                cancellationToken);

            await emailClient.DisconnectAsync(
                true, 
                cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while processing email for {id}", user.Id.value);
        }
    }
}
