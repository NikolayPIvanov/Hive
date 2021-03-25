using Billing.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Billing.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddBilling(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");
            var sqlServerConnectionString = configuration.GetConnectionString("DefaultConnection");
            if (useInMemory)
            {
                services.AddDbContext<BillingContext>(options =>
                    options.UseInMemoryDatabase("LooselyHive"));
            }
            else
            {
                services.AddDbContext<BillingContext>(options =>
                    options.UseSqlServer(
                        sqlServerConnectionString,
                        b => b.MigrationsAssembly(typeof(BillingContext).Assembly.FullName)));
            }

            services.AddScoped<IBillingContext>(provider => provider.GetService<BillingContext>());

            return services;
        }
    }
}