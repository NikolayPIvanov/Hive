using Hive.Common.Application.Publisher;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Infrastructure.MessageBroker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.Gig.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddGigsInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");
            var sqlServerConnectionString = configuration.GetConnectionString("DefaultConnection");
            if (useInMemory)
            {
                services.AddDbContext<GigManagementContext>(options =>
                    options.UseInMemoryDatabase("LooselyHive"));
            }
            else
            {
                services.AddDbContext<GigManagementContext>(options =>
                    options.UseSqlServer(
                        sqlServerConnectionString,
                        b => b.MigrationsAssembly(typeof(GigManagementContext).Assembly.FullName)));
            }

            services.AddCap(x =>
            {
                x.UseEntityFramework<GigManagementContext>();

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

            services.AddScoped<IGigManagementContext>(provider => provider.GetService<GigManagementContext>());
            services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

            return services;
        }
    }
}