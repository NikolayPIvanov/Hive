using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Seller.Contracts;

namespace Hive.Seller
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddSeller(this IServiceCollection services)
        {
            services.AddDbContext<SellerDbContext>(config =>
                config.UseSqlite("Data Source=AspIdUsers.db;"));
            services.AddScoped<ISellerRepository, SellerRepository>();
            return services;
        }
    }
}