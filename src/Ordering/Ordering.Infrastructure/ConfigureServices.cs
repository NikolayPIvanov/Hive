using BuildingBlocks.Core.Email;
using BuildingBlocks.Core.FileStorage;
using BuildingBlocks.Core.Interfaces;
using BuildingBlocks.Core.MessageBus;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Interfaces;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddOrderingInfrastructure(this IServiceCollection services, IConfiguration configuration)
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

            services.AddFileStorage(configuration);
            services.AddEmailService(configuration);
            services.AddRabbitMqBroker<OrderingDbContext>(useInMemory, sqlServerConnectionString, configuration);
            services.AddScoped<IOrderingContext>(provider => provider.GetService<OrderingDbContext>());

            return services;
        }
    }
}