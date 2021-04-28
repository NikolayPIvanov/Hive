using BuildingBlocks.Core.Email;
using BuildingBlocks.Core.MessageBus;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Services;
using Hive.UserProfile.Application.Interfaces;
using Hive.UserProfile.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.UserProfile.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddUserProfileInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");
            var sqlServerConnectionString = configuration.GetConnectionString("DefaultConnection");
            if (useInMemory)
            {
                services.AddDbContext<UserProfileDbContext>(options =>
                    options.UseInMemoryDatabase("LooselyHive"));
            }
            else
            {
                services.AddDbContext<UserProfileDbContext>(options =>
                    options.UseSqlServer(
                        sqlServerConnectionString,
                        b => b.MigrationsAssembly(typeof(UserProfileDbContext).Assembly.FullName)));
            }
            
            services.AddEmailService(configuration);
            services.AddRabbitMqBroker<UserProfileDbContext>(useInMemory, sqlServerConnectionString, configuration);
            services.AddScoped<IUserProfileDbContext>(provider => provider.GetService<UserProfileDbContext>());

            return services;
        }
    }
}