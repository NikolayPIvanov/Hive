using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace BuildingBlocks.Core.Caching
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services,
            IConfiguration configuration, string sectionName = "Redis")
        {
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(provider => 
                configuration.GetSection(sectionName).Get<RedisConfiguration>());

            return services;
        }
    }
}