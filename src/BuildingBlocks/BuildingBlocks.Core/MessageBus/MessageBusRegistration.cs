using BuildingBlocks.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Core.MessageBus
{
    public static class MessageBusRegistration
    {
        public static IServiceCollection AddRabbitMqBroker<TDbContext>(this IServiceCollection services, 
            bool useInMemoryDatabase, string sqlServerConnectionString, IConfiguration configuration) where TDbContext : DbContext
        {
            services.AddCap(options =>
            {
                options.UseEntityFramework<TDbContext>();

                if (!useInMemoryDatabase)
                {
                    options.UseSqlServer(sqlServerConnectionString);
                }
                
                var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
                
                options.UseRabbitMQ(ro =>
                {
                    ro.Password = rabbitMqSettings.Password;
                    ro.UserName = rabbitMqSettings.UserName;
                    ro.HostName = rabbitMqSettings.Hostname;
                    ro.Port = rabbitMqSettings.Port;
                    ro.VirtualHost = rabbitMqSettings.VirtualHost;
                });

                options.UseDashboard(opt => { opt.PathMatch = "/cap"; });
            });
            
            services.AddScoped<IIntegrationEventPublisher, RabbitMqPublisher>();

            return services;
        }
    }
}