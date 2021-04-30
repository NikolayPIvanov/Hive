using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Billing.Infrastructure.Persistence
{
    public static class PersistenceMigrator
    {
        public static async Task MigrateAsync(this IServiceProvider services,
            CancellationToken cancellationToken = default)
        {
            var context = services.GetRequiredService<BillingDbContext>();
                    
            if (context.Database.IsSqlServer())
            {
                await context.Database.MigrateAsync(cancellationToken);
            }
        }
    }
}