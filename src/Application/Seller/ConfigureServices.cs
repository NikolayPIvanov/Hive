using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Seller
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddSeller(this IServiceCollection services)
        {
            services.AddDbContext<SellerDbContext>(config => config.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<ISellerRepository, SellerRepository>();
            return services;
        }
    }
}