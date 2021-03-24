﻿using System.Reflection;
using FluentValidation;
using Hive.Common.Application.Behaviours;
using Hive.Gig.Application.IntegrationEvents;
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

            services.AddIntegrationEvents();
            

            return services;
        }
    }
}