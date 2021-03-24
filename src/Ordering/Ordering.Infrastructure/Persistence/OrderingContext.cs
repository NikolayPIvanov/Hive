using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderingContext : DbContext, IOrderingContext
    {
        private const string DefaultSchema = "ordering";
        
        public OrderingContext(
            DbContextOptions<OrderingContext> options) : base(options)
        {
        }

        public string Schema => DefaultSchema;
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<State> OrderStates { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<Resolution> Resolutions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(DefaultSchema);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}