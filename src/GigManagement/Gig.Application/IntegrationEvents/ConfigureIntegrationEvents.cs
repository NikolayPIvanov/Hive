using Microsoft.Extensions.DependencyInjection;

namespace Hive.Gig.Application.IntegrationEvents
{
    public static class ConfigureIntegrationEvents
    {
        public static IServiceCollection AddIntegrationEvents(this IServiceCollection services)
        {
            services.AddScoped<OrderCreatedIntegrationEventHandler>();

            return services;
        }
    }
}