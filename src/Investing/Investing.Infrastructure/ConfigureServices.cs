﻿using System;
using BuildingBlocks.Core.Caching;
using BuildingBlocks.Core.Email;
using BuildingBlocks.Core.MessageBus;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Services;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.Investing.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInvestingInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");
            var sqlServerConnectionString = configuration.GetConnectionString("DefaultConnection");
            if (useInMemory)
            {
                services.AddDbContext<InvestingDbContext>(options =>
                    options.UseInMemoryDatabase("LooselyHive"));
            }
            else
            {
                services.AddDbContext<InvestingDbContext>(options =>
                    options.UseSqlServer(
                        sqlServerConnectionString,
                        b =>
                        {
                            b.MigrationsAssembly(typeof(InvestingDbContext).Assembly.FullName);
                            b.EnableRetryOnFailure(
                                maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null
                            );
                        }));
            }

            services.AddRedis(configuration);
            services.AddSendGrid(configuration);
            services.AddMessagingBus<InvestingDbContext>(
                new StorageOptions(sqlServerConnectionString), 
                new MessagingOptions(configuration.GetValue<bool>("IsProduction")), 
                configuration);
            services.AddScoped<IInvestingDbContext>(provider => provider.GetService<InvestingDbContext>());

            return services;
        }
    }
}