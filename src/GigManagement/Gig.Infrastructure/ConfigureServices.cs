using Hive.Gig.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.Gig.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddGigsInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<GigManagementContext>(options =>
                    options.UseInMemoryDatabase("LooselyHive"));
            }
            else
            {
                services.AddDbContext<GigManagementContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(GigManagementContext).Assembly.FullName)));
            }

            services.AddScoped<IGigManagementContext>(provider => provider.GetService<GigManagementContext>());

            

            return services;
        }
    }
}