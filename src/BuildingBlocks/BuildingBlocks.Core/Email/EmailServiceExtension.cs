using BuildingBlocks.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Core.Email
{
    public static class EmailServiceExtension
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
            services.AddScoped<IEmailService, EmailService>();
            
            return services;
        }
        
        public static IServiceCollection AddSendGrid(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthMessageSenderOptions>(configuration.GetSection(nameof(AuthMessageSenderOptions)));
            services.AddScoped<IEmailService, EmailService>();
            
            return services;
        }
    }
}