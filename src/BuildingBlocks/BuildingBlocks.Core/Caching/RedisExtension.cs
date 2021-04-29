using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace BuildingBlocks.Core.Caching
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(provider => 
                configuration.GetSection("Redis").Get<RedisConfiguration>());

            return services;
        }
    }
}