using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.Common.Application
{
    public static class ScrutorRegister
    {
        public static IServiceCollection ScanFor<T>(this IServiceCollection services, Assembly[] assemblies)
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