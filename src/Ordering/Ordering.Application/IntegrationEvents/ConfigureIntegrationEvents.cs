using System.Reflection;
using DotNetCore.CAP;
using Hive.Common.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application.IntegrationEvents
{
    public static class ConfigureIntegrationEvents
    {
        public static IServiceCollection AddIntegrationEvents(this IServiceCollection services)
        {
            services.ScanFor<ICapSubscribe>(new[] {Assembly.GetExecutingAssembly()});

            return services;
        }
    }
}