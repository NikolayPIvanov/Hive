using System;
using BuildingBlocks.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Core.MessageBus
{
    public record StorageOptions(string ConnectionString, bool InMemoryStorage = false);
    public record MessagingOptions(bool IsProduction = false);
    
    public static class MessageBusRegistration
    {
        public static IServiceCollection AddMessagingBus<TDbContext>(this IServiceCollection services, 
            StorageOptions storageOptions, MessagingOptions messagingOptions,  IConfiguration configuration) where TDbContext : DbContext
        {
            services.AddCap(options =>
            {
                options.UseEntityFramework<TDbContext>();

                if (!storageOptions.InMemoryStorage && !string.IsNullOrEmpty(storageOptions.ConnectionString))
                {
                    options.UseSqlServer(storageOptions.ConnectionString);
                }
                else
                {
                    options.UseInMemoryStorage();
                }
                
                if (messagingOptions.IsProduction)
                {
                    var azureServiceBusSettings = configuration.GetSection(nameof(ServiceBusSettings)).Get<ServiceBusSettings>();
                    options.UseAzureServiceBus(x =>
                    {
                        x.ConnectionString = azureServiceBusSettings.ConnectionString;
                        x.EnableSessions = azureServiceBusSettings.EnableSessions;
                        x.TopicPath = azureServiceBusSettings.TopicPath;
                    });
                    
                    services.AddScoped<IIntegrationEventPublisher, ServiceBusPublisher>();
                }
                else
                {
                    var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
                
                    options.UseRabbitMQ(ro =>
                    {
                        ro.Password = rabbitMqSettings.Password;
                        ro.UserName = rabbitMqSettings.UserName;
                        ro.HostName = rabbitMqSettings.Hostname;
                        ro.Port = rabbitMqSettings.Port;
                        ro.VirtualHost = rabbitMqSettings.VirtualHost;
                        ro.ConnectionFactoryOptions = factory =>
                        {
                            factory.AutomaticRecoveryEnabled = true;
                            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(30);
                        };
                    });
                    
                    services.AddScoped<IIntegrationEventPublisher, RabbitMqPublisher>();
                }

                options.UseDashboard(opt =>
                {
                    opt.PathMatch = "/cap";
                });
            });
            

            return services;
        }
    }
}