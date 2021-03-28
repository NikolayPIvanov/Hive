using Billing.Application.Interfaces;
using Billing.Infrastructure.MessageBroker;
using Common.Infrastructure.Services;
using Hive.Common.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Billing.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddBillingInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");
            var sqlServerConnectionString = configuration.GetConnectionString("DefaultConnection");
            if (useInMemory)
            {
                services.AddDbContext<BillingContext>(options =>
                    options.UseInMemoryDatabase("LooselyHive"));
            }
            else
            {
                services.AddDbContext<BillingContext>(options =>
                    options.UseSqlServer(
                        sqlServerConnectionString,
                        b => b.MigrationsAssembly(typeof(BillingContext).Assembly.FullName)));
            }
            
            
            services.AddCap(x =>
            {
                x.UseEntityFramework<BillingContext>();

                if (!useInMemory)
                {
                    x.UseSqlServer(sqlServerConnectionString);
                }

                x.UseRabbitMQ(ro =>
                {
                    ro.Password = "admin";
                    ro.UserName = "admin";
                    ro.HostName = "localhost";
                    ro.Port = 5672;
                    ro.VirtualHost = "/";
                });

                x.UseDashboard(opt => { opt.PathMatch = "/cap"; });
            });

            services.AddScoped<IBillingContext>(provider => provider.GetService<BillingContext>());
            services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
            services.AddScoped<IDateTimeService, DateTimeService>();


            return services;
        }
    }
}