using Hive.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hive.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserAccountType> UserAccountTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(e =>
            {
                e.HasOne(x => x.AccountType)
                    .WithMany()
                    .HasForeignKey(ac => ac.AccountTypeId);
            });

            builder.Entity<UserAccountType>(b =>
            {
                b.HasIndex(x => new {x.Type, x.UserId}).IsUnique();
            });
        }
    }
}