using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BuildingBlocks.Core.Email
{
    public class SendGridService : IEmailService
    {
        private readonly AuthMessageSenderOptions _optionsAccessor;

        public SendGridService(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        }
        
        public async Task SendAsync(IEnumerable<string> recipients, string subject, string message, CancellationToken cancellationToken = default)
        {
            var emailTasks =
                recipients.Select(recipient => Execute(_optionsAccessor.SendGridKey, subject, message, recipient, cancellationToken));

            await Task.WhenAll(emailTasks);
        }
        
        private Task Execute(string apiKey, string subject, string message, string email, CancellationToken cancellationToken)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(_optionsAccessor.SendGridSenderEmail, _optionsAccessor.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg, cancellationToken);
        }
    }
}