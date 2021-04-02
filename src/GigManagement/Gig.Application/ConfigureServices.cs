using System.Reflection;
using DotNetCore.CAP;
using FluentValidation;
using Hive.Common.Core;
using Hive.Common.Core.Behaviours;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.Gig.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddGigsManagement(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

            // Integration Events
            services.ScanFor<ICapSubscribe>(new[] {Assembly.GetExecutingAssembly()});
            

            return services;
        }
    }
}