using Hive.Common.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Interfaces;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Services;

namespace Ordering.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddOrdering(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");
            var sqlServerConnectionString = configuration.GetConnectionString("DefaultConnection");
            if (useInMemory)
            {
                services.AddDbContext<OrderingDbContext>(options =>
                    options.UseInMemoryDatabase("LooselyHive"));
            }
            else
            {
                services.AddDbContext<OrderingDbContext>(options =>
                    options.UseSqlServer(
                        sqlServerConnectionString,
                        b => b.MigrationsAssembly(typeof(OrderingDbContext).Assembly.FullName)));
            }

            services.AddCap(x =>
            {
                x.UseEntityFramework<OrderingDbContext>();

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

            services.AddScoped<IOrderingContext>(provider => provider.GetService<OrderingDbContext>());
            services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}