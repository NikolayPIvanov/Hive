using System;
using BuildingBlocks.Core.Caching;
using BuildingBlocks.Core.Email;
using BuildingBlocks.Core.FileStorage;
using BuildingBlocks.Core.MessageBus;
using Hive.UserProfile.Application.Interfaces;
using Hive.UserProfile.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.UserProfile.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddUserProfileInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");
            var sqlServerConnectionString = configuration.GetConnectionString("DefaultConnection");
            if (useInMemory)
            {
                services.AddDbContext<UserProfileDbContext>(options =>
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            }
            else
            {
                services.AddDbContext<UserProfileDbContext>(options =>
                    options.UseSqlServer(
                        sqlServerConnectionString,
                        b =>
                        {
                            b.MigrationsAssembly(typeof(UserProfileDbContext).Assembly.FullName);
                            b.EnableRetryOnFailure(
                                maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null
                            );
                        }));
            }
            
            var isProduction = configuration.GetValue<bool>("IsProduction");

            
            services.AddRedis(configuration);
            services.AddFileStorage(configuration);
            
            services.AddMessagingBus<UserProfileDbContext>(
                new StorageOptions(sqlServerConnectionString),
                new MessagingOptions(isProduction),
                configuration);
            
            services.AddScoped<IUserProfileDbContext>(provider => provider.GetService<UserProfileDbContext>());

            return services;
        }
    }
}