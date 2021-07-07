using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Hive.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailService _emailService;

        public EmailSender(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return _emailService.SendAsync(new[] {email}, subject, message);
        }
    }
}