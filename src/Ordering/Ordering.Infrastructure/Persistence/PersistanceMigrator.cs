using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Persistence
{
    public static class PersistenceMigrator
    {
        public static async Task MigrateAsync(this IServiceProvider services,
            CancellationToken cancellationToken = default)
        {
            var context = services.GetRequiredService<OrderingDbContext>();
                    
            if (context.Database.IsSqlServer())
            {
                await context.Database.MigrateAsync(cancellationToken);
            }
        }
    }
}