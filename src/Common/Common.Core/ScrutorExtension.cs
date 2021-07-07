using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.Common.Core
{
    public static class ScrutorExtension
    {
        public static IServiceCollection AddOfType<T>(this IServiceCollection services, Assembly[] assemblies)
        {
            services.Scan(scan =>
                scan.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo<T>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            return services;
        }
    }
}