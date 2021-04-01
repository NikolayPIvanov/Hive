using Common.Infrastructure.Services;
using Hive.Common.Application.Interfaces;
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
                services.AddDbContext<UserProfileContext>(options =>
                    options.UseInMemoryDatabase("LooselyHive"));
            }
            else
            {
                services.AddDbContext<UserProfileContext>(options =>
                    options.UseSqlServer(
                        sqlServerConnectionString,
                        b => b.MigrationsAssembly(typeof(UserProfileContext).Assembly.FullName)));
            }
            
            
            services.AddCap(x =>
            {
                x.UseEntityFramework<UserProfileContext>();

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

            services.AddScoped<IUserProfileContext>(provider => provider.GetService<UserProfileContext>());
            services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
            services.AddScoped<IDateTimeService, DateTimeService>();


            return services;
        }
    }
}