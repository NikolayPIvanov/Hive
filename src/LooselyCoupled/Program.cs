using System;
using System.Linq;
using System.Threading.Tasks;
using Billing.Infrastructure.Persistence;
using Hive.Gig.Infrastructure.Persistence;
using Hive.Investing.Infrastructure.Persistence;
using Hive.UserProfile.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Persistence;

namespace Hive.LooselyCoupled
{
    
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var orderingDbContext = services.GetRequiredService<OrderingDbContext>();
                    var gigManagementDbContext = services.GetRequiredService<GigManagementDbContext>();
                    var investingDbContext = services.GetRequiredService<InvestingDbContext>();
                    var userProfileDbContext = services.GetRequiredService<UserProfileDbContext>();
                    var billingDbContext = services.GetRequiredService<BillingDbContext>();
                    DbContext[] contexts = {
                        orderingDbContext, gigManagementDbContext, investingDbContext, userProfileDbContext,
                        billingDbContext
                    };

                    foreach (var dbContext in contexts)
                    {
                        if (dbContext.Database.IsSqlServer())
                        {
                            dbContext.Database.Migrate();
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}