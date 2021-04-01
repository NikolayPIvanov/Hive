﻿using System.Reflection;
using DotNetCore.CAP;
using FluentValidation;
using Hive.Common.Application;
using Hive.Common.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.UserProfile.Application
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