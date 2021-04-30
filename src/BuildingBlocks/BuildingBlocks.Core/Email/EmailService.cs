using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace BuildingBlocks.Core.Email
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        
        public SmtpEmailService(IOptions<EmailSettings> options)
        {
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }
        
        public async Task SendAsync(IEnumerable<string> recipients, string subject, string message, CancellationToken cancellationToken)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_settings.FromEmail));
            recipients.ToList().ForEach(recipient => email.To.Add(MailboxAddress.Parse(recipient)));
            email.Subject = subject;
            
            // TODO: Based on type
            email.Body = new TextPart(TextFormat.Plain) { Text = message };
            
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}