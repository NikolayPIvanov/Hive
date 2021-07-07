using System;
using Billing.Application.Interfaces;
using Billing.Infrastructure.Persistence;
using BuildingBlocks.Core.Caching;
using BuildingBlocks.Core.Email;
using BuildingBlocks.Core.MessageBus;
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
                services.AddDbContext<BillingDbContext>(options =>
                    options.UseInMemoryDatabase("LooselyHive"));
            }
            else
            {
                services.AddDbContext<BillingDbContext>(options =>
                    options.UseSqlServer(
                        sqlServerConnectionString,
                        b =>
                        {
                            b.MigrationsAssembly(typeof(BillingDbContext).Assembly.FullName);
                            b.EnableRetryOnFailure(
                                maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null
                            );
                        }));
            }

            services.AddRedis(configuration);
            services.AddSendGrid(configuration);
            services.AddMessagingBus<BillingDbContext>(
                new StorageOptions(sqlServerConnectionString), 
                new MessagingOptions(configuration.GetValue<bool>("IsProduction")), 
                configuration);
            services.AddScoped<IBillingDbContext>(provider => provider.GetService<BillingDbContext>());
            
            return services;
        }
    }
}