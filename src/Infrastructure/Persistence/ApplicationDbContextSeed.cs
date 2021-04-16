using Hive.Domain.Entities;
using Hive.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Entities.Orders;

namespace Hive.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IApplicationDbContext context)
        {
            await SeedAdmin(userManager, roleManager, context);
            
            await SeedOfType(AccountType.Buyer, "buyer1@buyer.com", userManager, roleManager, context);
            await SeedOfType(AccountType.Buyer, "buyer2@buyer.com", userManager, roleManager, context);
            
            await SeedOfType(AccountType.Seller, "seller1@seller.com", userManager, roleManager, context);
            await SeedOfType(AccountType.Seller, "seller2@seller.com", userManager, roleManager, context);
        }

        private static async Task SeedAdmin(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IApplicationDbContext context)
        {
            var administratorRole = new IdentityRole("Administrator");

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
            }

            var administrator = new ApplicationUser(AccountType.Buyer) { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "Administrator1!");
                await userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
            }

            administrator.Buyer = new Buyer(administrator.Id);
            await context.SaveChangesAsync(default);
        }

        private static async Task SeedOfType(AccountType type, string email, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IApplicationDbContext context)
        {
            var role = new IdentityRole(type.ToString());
            if (roleManager.Roles.All(r => r.Name != role.Name))
            {
                await roleManager.CreateAsync(role);
            }
            
            var user = new ApplicationUser(type) { UserName = email, Email = email };
            if (userManager.Users.All(u => u.UserName != user.UserName))
            {
                await userManager.CreateAsync(user, "Administrator1!");
                await userManager.AddToRolesAsync(user, new [] { role.Name });
            }

            switch (type)
            {
                case AccountType.Buyer: user.Buyer = new Buyer(user.Id); break;
                case AccountType.Seller: user.Seller = new Seller(user.Id);
                    break;
            }

            await context.SaveChangesAsync(default);

        }
    }
}
