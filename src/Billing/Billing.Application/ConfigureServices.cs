using System.Reflection;
using Billing.Application.IntegrationEvents;
using DotNetCore.CAP;
using FluentValidation;
using Hive.Common.Core;
using Hive.Common.Core.Behaviours;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Billing.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddBillingApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

            services.ScanFor<ICapSubscribe>(new []{ Assembly.GetExecutingAssembly() });
            
            return services;
        }
    }
}