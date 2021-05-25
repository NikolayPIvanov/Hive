using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Identity.Data;
using Hive.Identity.Models;
using Hive.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Hive.Identity.Jobs
{
    public class RoleSeeder : IHostedService, IDisposable
    {
        public IServiceProvider Services { get; }

        public RoleSeeder(IServiceProvider services)
        {
            Services = services;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = Services.CreateScope())
            {
                var userManager = 
                    scope.ServiceProvider
                        .GetRequiredService<UserManager<ApplicationUser>>();
                
                var roleManager = 
                    scope.ServiceProvider
                        .GetRequiredService<RoleManager<IdentityRole>>();
                
                var context = 
                    scope.ServiceProvider
                        .GetRequiredService<ApplicationDbContext>();

                var identityDispatcher =
                    scope.ServiceProvider
                        .GetRequiredService<IIdentityDispatcher>();

                var redisCacheClient =
                    scope.ServiceProvider
                        .GetRequiredService<IRedisCacheClient>();
                
                await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager, context,
                    identityDispatcher, redisCacheClient);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public void Dispose()
        { }
    }
}