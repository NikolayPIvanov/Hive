using System.Linq;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Entities.Orders;
using Hive.Infrastructure.Identity;
using Hive.Infrastructure.Persistence;
using Identity.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
            IdentityDbContext context, IApplicationDbContext applicationDbContext)
        {
            await SeedOfType(userManager, roleManager, context, applicationDbContext, AccountType.Buyer);
            await SeedOfType(userManager, roleManager, context, applicationDbContext, AccountType.Seller);
        }
        
        public static async Task SeedOfType(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IdentityDbContext identityDbContext, IApplicationDbContext applicationDbContext, AccountType type)
        {
            var role = new IdentityRole(type.ToString());
            if (roleManager.Roles.All(r => r.Name != role.Name))
            {
                await roleManager.CreateAsync(role);
            }
            
            await CreateOfType(userManager, role, identityDbContext, applicationDbContext, type, "jack", "Jack123@jack.com");
            await CreateOfType(userManager, role, identityDbContext, applicationDbContext, type, "mike", "Jack1234@jack.com");
        }

        private static async Task CreateOfType(
            UserManager<ApplicationUser> userManager, IdentityRole buyerRole, IdentityDbContext identityDbContext,
            IApplicationDbContext applicationDbContext, AccountType type, string username, string email)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == username) ?? new ApplicationUser(type)
            {
                UserName = username,
                Email = email
            };

            if (userManager.Users.All(u => u.UserName != user.UserName))
            {
                await userManager.CreateAsync(user, "Administrator1!");
                await userManager.AddToRolesAsync(user, new[] {buyerRole.Name});
            }

            switch (type)
            {
                case AccountType.Buyer:
                    applicationDbContext.Buyers.Add(new Buyer(user.Id));
                    break;
                case AccountType.Seller:
                    applicationDbContext.Sellers.Add(new Seller(user.Id));
                    break;
            }
            
            await identityDbContext.SaveChangesAsync();
            await applicationDbContext.SaveChangesAsync(default);
        }
    }
}
