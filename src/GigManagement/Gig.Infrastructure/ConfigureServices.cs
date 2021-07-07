using System;
using BuildingBlocks.Core.Caching;
using BuildingBlocks.Core.Email;
using BuildingBlocks.Core.FileStorage;
using BuildingBlocks.Core.MessageBus;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Services;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Infrastructure.Persistence;
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
                services.AddDbContext<GigManagementDbContext>(options =>
                    options.UseInMemoryDatabase("LooselyHive"));
            }
            else
            {
                services.AddDbContext<GigManagementDbContext>(options =>
                    options.UseSqlServer(
                        sqlServerConnectionString,
                        b =>
                        {
                            b.MigrationsAssembly(typeof(GigManagementDbContext).Assembly.FullName);
                            b.EnableRetryOnFailure(
                                maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null
                            );
                        }));
            }

            services.AddSendGrid(configuration);
            services.AddRedis(configuration);
            services.AddFileStorage(configuration);
            services.AddMessagingBus<GigManagementDbContext>(
                new StorageOptions(sqlServerConnectionString), 
                new MessagingOptions(configuration.GetValue<bool>("IsProduction")), 
                configuration);
            services.AddScoped<IGigManagementDbContext>(provider => provider.GetService<GigManagementDbContext>());
            
            return services;
        }
    }
}