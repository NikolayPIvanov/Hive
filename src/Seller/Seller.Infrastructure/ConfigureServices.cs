using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.Seller.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddSellerInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<SellerDbContext>(options =>
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            }
            else
            {
                services.AddDbContext<SellerDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(SellerDbContext).Assembly.FullName)));
            }

            return services;
        }
    }
}